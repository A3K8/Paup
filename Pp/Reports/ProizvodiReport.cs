using iTextSharp.text;
using iTextSharp.text.pdf;
using Pp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Pp.Reports
{
    public class ProizvodiReport
    {

        public byte[] Podaci { get; private set; }

        private PdfPCell GenerirajCeliju(string sadrzaj, Font font, BaseColor boja, bool wrap)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(sadrzaj, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.Padding = 5;
            c1.NoWrap = wrap;
            c1.Border = Rectangle.BOTTOM_BORDER;
            c1.BorderColor = BaseColor.LIGHT_GRAY;
            return c1;
        }
        public void ListaProizvoda(List<Proizvodi> proizvodi)
        {
            //fontovi
            BaseFont bfontZaglavlje = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            BaseFont bfontText = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            BaseFont bfontPodnozje = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

            Font fontZaglavlje = new Font(bfontZaglavlje, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font fontZaglavljeBold = new Font(bfontZaglavlje, 12, Font.BOLD, BaseColor.DARK_GRAY);
            Font fontNaslov = new Font(bfontText, 14, Font.BOLDITALIC, BaseColor.DARK_GRAY);
            Font fontTablicaZaglavlje = new Font(bfontText, 10, Font.BOLD, BaseColor.WHITE);
            Font fontTekst = new Font(bfontText, 10, Font.NORMAL, BaseColor.BLACK);

            BaseColor tPozadinaZaglavlje = new BaseColor(255, 50, 50);

            BaseColor tPozadinaSadrzaj = BaseColor.WHITE;

            using (MemoryStream mstream = new MemoryStream())
            {
                using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
                {
                    PdfWriter.GetInstance(pdfDokument, mstream).CloseStream = false;

                    pdfDokument.Open();

                    PdfPTable tZaglavlje = new PdfPTable(2);
                    tZaglavlje.HorizontalAlignment = Element.ALIGN_LEFT;
                    tZaglavlje.DefaultCell.Border = Rectangle.NO_BORDER;
                    tZaglavlje.WidthPercentage = 100f;
                    float[] sirinaKolonaZaglavlja = new float[] { 1f, 3f };
                    tZaglavlje.SetWidths(sirinaKolonaZaglavlja);

                   

                    
                    Paragraph info = new Paragraph();
                    info.Alignment = Element.ALIGN_RIGHT;
                  
                    info.SetLeading(0, 1.2f);
                    info.Add(new Chunk("Međimursko Veleučilište u Čakovcu \n", fontZaglavljeBold));
                    info.Add(new Chunk("Bana Josipa Jelačića 22a \n" + "Čakovec \n", fontZaglavlje));

                    PdfPCell cInfo = new PdfPCell();
                    cInfo.AddElement(info);
                    cInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cInfo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(info);

                   
                    pdfDokument.Add(tZaglavlje);

                    //naslov
                    Paragraph pNaslov = new Paragraph("Popis proizvoda", fontNaslov);
                    pNaslov.Alignment = Element.ALIGN_CENTER;
                    pNaslov.SpacingBefore = 20;
                    pNaslov.SpacingAfter = 20;
                    pdfDokument.Add(pNaslov);

                    //tablica za proizvode
                    PdfPTable t = new PdfPTable(4); 
                    t.WidthPercentage = 100;
                    t.SetWidths(new float[] { 1, 3, 2, 1 });

                    t.AddCell(GenerirajCeliju("ID proizvoda:", fontTablicaZaglavlje, tPozadinaZaglavlje, false));
                    t.AddCell(GenerirajCeliju("Vrsta", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Materijal", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Cijena", fontTablicaZaglavlje, tPozadinaZaglavlje, true));

                    //redovi
                    int i = 1;
                    foreach (Proizvodi p in proizvodi)
                    {
                        t.AddCell(GenerirajCeliju(p.Id.ToString(), fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(p.Vrsta, fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(p.Materijal, fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(p.Cijena.ToString(), fontTekst, tPozadinaSadrzaj, false));
                        i++;
                    }

                    pdfDokument.Add(t);
                    pNaslov = new Paragraph("Čakovec, " + System.DateTime.Now.ToString("dd.MM.yyyy"), fontTekst);
                    pNaslov.Alignment = Element.ALIGN_RIGHT;
                    pNaslov.SpacingBefore = 30;
                    pdfDokument.Add(pNaslov);
                }

                Podaci = mstream.ToArray();

                using (var reader = new PdfReader(Podaci))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var stamper = new PdfStamper(reader, ms))
                        {
                            int PageCount = reader.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                            {
                                Rectangle pageSize = reader.GetPageSize(i);
                                PdfContentByte canvas = stamper.GetOverContent(i);

                                canvas.BeginText();
                                canvas.SetFontAndSize(bfontPodnozje, 10);

                                canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {PageCount}", pageSize.Right - 50, 30, 0);
                                canvas.EndText();
                            }
                        }
                        Podaci = ms.ToArray();
                    }
                }
            }
        }
    }
}