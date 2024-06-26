using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Pp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pp.Controllers
{
    public class PonudaController : Controller
    {

        BazaDbContext bazaPodataka = new BazaDbContext();

        [HttpPost]
        public ActionResult GeneratePdf(FormCollection form)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                Paragraph companyName = new Paragraph("LB-mont", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                companyName.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(companyName);
                pdfDoc.Add(new Paragraph(" "));

                // Dodavanje naslova
                pdfDoc.Add(new Paragraph("Zatraži ponudu", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)));
                pdfDoc.Add(new Paragraph(" ")); 

                var labels = new Dictionary<string, string>
                {
                    { "selection", "Odabir" },
                    { "customMessage", "Vlastita poruka" },
                    { "fullName", "Ime i prezime" },
                    { "email", "E-mail adresa" }
                };

                var request = new Request
                {
                    Selection = form["selection"],
                    CustomMessage = form["customMessage"],
                    FullName = form["fullName"],
                    Email = form["email"]
                };

                int windowQuantity = 0;
                int doorQuantity = 0;

                using (var db = bazaPodataka)
                {
                    db.Requests.Add(request);
                    db.SaveChanges();

                    int requestId = request.Id;

                    if (int.TryParse(form["windowQuantity"], out windowQuantity) && windowQuantity > 0)
                    {
                        for (int i = 1; i <= windowQuantity; i++)
                        {
                            var window = new Window
                            {
                                RequestId = requestId,
                                Width = int.Parse(form[$"prozorWidth{i}"]),
                                Height = int.Parse(form[$"prozorHeight{i}"]),
                                Material = form[$"prozorMaterial{i}"],
                                Color = form[$"prozorColor{i}"],
                                Type = form[$"prozorType{i}"],
                                Wing = form[$"prozorWing{i}"]
                            };
                            db.Windows.Add(window);
                        }
                    }

                    if (int.TryParse(form["doorQuantity"], out doorQuantity) && doorQuantity > 0)
                    {
                        for (int i = 1; i <= doorQuantity; i++)
                        {
                            var door = new Door
                            {
                                RequestId = requestId,
                                Width = int.Parse(form[$"vrataWidth{i}"]),
                                Height = int.Parse(form[$"vrataHeight{i}"]),
                                Material = form[$"vrataMaterial{i}"],
                                Color = form[$"vrataColor{i}"],
                                Type = form[$"vrataType{i}"]
                            };
                            db.Doors.Add(door);
                        }
                    }

                    db.SaveChanges();
                }

                foreach (var key in labels.Keys)
                {
                    if (form.AllKeys.Contains(key))
                    {
                        pdfDoc.Add(new Paragraph($"{labels[key]}: {form[key]}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                    }
                }

                pdfDoc.Add(new Paragraph(" ")); 

                // Dinamičko dodavanje prozora
                if (windowQuantity > 0)
                {
                    for (int i = 1; i <= windowQuantity; i++)
                    {
                        PdfPTable windowTable = new PdfPTable(7);
                        windowTable.WidthPercentage = 100;
                        windowTable.SpacingBefore = 10f;
                        windowTable.SpacingAfter = 10f;

                        AddTableHeader(windowTable, new string[] { $"Prozor {i}", "Širina (cm)", "Visina (cm)", "Materijal", "Boja", "Vrsta", "Vrsta krila" });

                        windowTable.AddCell(new Phrase($"Prozor {i}", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        windowTable.AddCell(new Phrase(form[$"prozorWidth{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        windowTable.AddCell(new Phrase(form[$"prozorHeight{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        windowTable.AddCell(new Phrase(form[$"prozorMaterial{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        windowTable.AddCell(new Phrase(form[$"prozorColor{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        windowTable.AddCell(new Phrase(form[$"prozorType{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        windowTable.AddCell(new Phrase(form[$"prozorWing{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));

                        pdfDoc.Add(windowTable);
                    }
                }

                // Dinamičko dodavanje vrata
                if (doorQuantity > 0)
                {
                    for (int i = 1; i <= doorQuantity; i++)
                    {
                        PdfPTable doorTable = new PdfPTable(6);
                        doorTable.WidthPercentage = 100;
                        doorTable.SpacingBefore = 10f;
                        doorTable.SpacingAfter = 10f;

                        AddTableHeader(doorTable, new string[] { $"Vrata {i}", "Širina (cm)", "Visina (cm)", "Materijal", "Boja", "Vrsta" });

                        doorTable.AddCell(new Phrase($"Vrata {i}", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        doorTable.AddCell(new Phrase(form[$"vrataWidth{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        doorTable.AddCell(new Phrase(form[$"vrataHeight{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        doorTable.AddCell(new Phrase(form[$"vrataMaterial{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        doorTable.AddCell(new Phrase(form[$"vrataColor{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        doorTable.AddCell(new Phrase(form[$"vrataType{i}"], FontFactory.GetFont(FontFactory.HELVETICA, 10)));

                        pdfDoc.Add(doorTable);
                    }
                }

                pdfDoc.Add(new Paragraph(" "));
                pdfDoc.Add(new Paragraph("Hvala na zahtjevu! Vaša ponuda stiže kroz par dana na e-mail!", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));

                pdfDoc.Close();
                writer.Close();

                // Vraćanje PDF-a kao rezultat
                return File(stream.ToArray(), "application/pdf", "Ponuda.pdf");
            }
        }

        private void AddTableHeader(PdfPTable table, string[] headers)
        {
            foreach (var header in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new BaseColor(211, 211, 211); 
                table.AddCell(cell);
            }
        }
    }
}