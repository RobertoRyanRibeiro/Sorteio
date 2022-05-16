using OfficeOpenXml;
using Sorteio.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorteio.Data
{
    public static class DB
    {
        public static List<Funcionario> Ler(string path)
        {
            FileInfo file = new FileInfo(Path.GetFullPath(path));

            List<Funcionario> funcs = new List<Funcionario>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int columCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    var funcionario = new Funcionario();
                    funcionario.Nome = worksheet.Cells[row, 1].Value.ToString();
                    funcionario.ID = row - 1;
                    if (worksheet.Cells[row, 2].Value != null)
                        funcionario.Venceu = bool.Parse(worksheet.Cells[row, 2].Value.ToString());

                    funcs.Add(funcionario);
                }
            }

            return funcs;
        }

        public static void Alterar(Funcionario func, string path, int row)
        {
            FileInfo file = new FileInfo(Path.GetFullPath(path));

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                if (func.Venceu)
                    worksheet.Cells[row, 2].Value = "true";
                else
                    worksheet.Cells[row, 2].Value = "false";
                package.Save(); 
            }
        }

        public static void Reset(string path, int row)
        {
            FileInfo file = new FileInfo(Path.GetFullPath(path));

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[row, 2].Value = null;
                package.Save();
            }
        }

    }
}
