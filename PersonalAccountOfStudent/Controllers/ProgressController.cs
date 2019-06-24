using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PersonalAccountOfStudent.Models;
using PersonalAccountOfStudent.TablesGateway;

namespace PersonalAccountOfStudent.Controllers
{
    [Authorize]
    public class ProgressController : Controller
    {
        private IHostingEnvironment Env { get; }
        private SchoolContext db;
        private FileInfo fileInfo;
        private string path;
        private IDictionary<string, List<double>> Assessments;

        public ProgressController(IHostingEnvironment env, SchoolContext context)
        {
            Env = env;
            db = context;
            path = Path.Combine(Env.WebRootPath, "files/progress");
            Assessments = new Dictionary<string, List<double>> {
                { "Русский язык", new List<double>() },
                { "Литература",new List<double>()},
                { "Алгебра", new List<double>()},
                { "Геометрия",new List<double>()},
                { "История", new List<double>()},
                { "Обществознание", new List<double>()},
                { "Физика", new List<double>()},
                { "ОБЖ", new List<double>()},
                { "Информатика",new List<double>()},
                { "Биология",new List<double>()},
                { "Музыка",new List<double>()},
                { "ИЗО",new List<double>()},
                { "Физ-ра",new List<double>()},
                { "Англ. язык",new List<double>()},
                { "Технология",new List<double>()}
            };
        }

        public IActionResult ViewProgress()
        {
            var user = db?.Users?.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name && u.UserType == "Student");
            if (user != null)
            {
                foreach (var subject in Assessments.Keys)
                {
                    Assessments[subject].AddRange(AssessmentGateway.FindAssessments(db, user.GUID, subject)
                        .Select(assessment => assessment.Mark));
                }

                var studentFIO = db.Students.FirstOrDefault(student => student.UserGUID == user.GUID).FIO;
                string fullPath = Path.Combine(path, String.Format("Успеваемость_{0}.xlsx", studentFIO));
                fileInfo = new FileInfo(fullPath);

                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    stream.SetLength(0);

                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Успеваемость");
                    IRow row = excelSheet.CreateRow(0);
                    row.CreateCell(0).SetCellValue(studentFIO);

                    row = excelSheet.CreateRow(1);
                    row.CreateCell(0).SetCellValue("Предмет");
                    row.CreateCell(1).SetCellValue("Средняя оценка");

                    int i = 2;
                    foreach (var subject in Assessments.Keys)
                    {
                        row = excelSheet.CreateRow(i);
                        row.CreateCell(0).SetCellValue(subject);
                        if (Assessments[subject].Count != 0)
                            row.CreateCell(1).SetCellValue(Math.Round(Assessments[subject].Average(), 1));
                        else
                            row.CreateCell(1).SetCellValue("0");
                        i++;
                    }

                    workbook.Write(stream);
                }

                StringBuilder sb = new StringBuilder();
                if (System.IO.File.Exists(fullPath) && fileInfo.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        var memory = new MemoryStream();
                        stream.CopyTo(memory);
                        memory.Position = 0;

                        ISheet sheet;
                        string fileExtension = fileInfo.Extension;
                        if (fileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(memory);
                            sheet = hssfwb.GetSheetAt(0);
                        }
                        else
                        {
                            XSSFWorkbook wssfwb = new XSSFWorkbook(memory);
                            sheet = wssfwb.GetSheetAt(0);
                        }

                        sb.Append("<table class='table'><tr>");

                        IRow fioRow = sheet.GetRow(0);
                        NPOI.SS.UserModel.ICell cell = fioRow.GetCell(0);
                        if (cell != null && !String.IsNullOrEmpty(cell.ToString()))
                            sb.Append("<th colspan='2'>" + cell.ToString() + "</th>");

                        sb.Append("</tr>");
                        sb.AppendLine("<tr>");

                        IRow secondRow = sheet.GetRow(1);
                        int cellCount = secondRow.LastCellNum;
                        for (int i = 0; i < cellCount; i++)
                        {
                            NPOI.SS.UserModel.ICell cell1 = secondRow.GetCell(i);
                            if (cell1 != null && !String.IsNullOrEmpty(cell1.ToString()))
                                sb.Append("<th>" + cell1.ToString() + "</th>");
                        }

                        sb.Append("</tr>");

                        for (int i = sheet.FirstRowNum + 2; i <= sheet.LastRowNum; i++)
                        {
                            sb.AppendLine("<tr>");

                            IRow row = sheet.GetRow(i);
                            if (row == null || row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                            }

                            sb.AppendLine("</tr>");
                        }

                        sb.AppendLine("</tr>");
                    }

                    ViewData["FileExist"] = true;
                    ViewData["Grid"] = sb.ToString();
                }
                else
                {
                    ViewData["FileExist"] = false;
                    ViewData["Grid"] = "";
                }

                ViewData["UserType"] = user.UserType;
            }
            else
            {
                ViewData["FileExist"] = false;
                ViewData["Grid"] = "";
            }

            return View();
        }

        public async Task<IActionResult> DownloadProgress()
        {
            var user = db?.Users?.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name && u.UserType == "Student");
            if (user != null)
            {
                var studentFIO = db.Students.FirstOrDefault(student => student.UserGUID == user.GUID).FIO;
                string fullPath = Path.Combine(path, String.Format("Успеваемость_{0}.xlsx", studentFIO));
                fileInfo = new FileInfo(fullPath);

                if (System.IO.File.Exists(fullPath) && fileInfo.Length > 0)
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;

                    string extension = fileInfo.Extension;
                    RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false);
                    object value = key != null ? key.GetValue("Content Type", null) : null;
                    string mimeType = value != null ? value.ToString() : String.Empty;

                    if (!String.IsNullOrEmpty(mimeType))
                        return File(memory, mimeType, fileInfo.Name);

                    return Content("trouble...");
                }

                return Content(StatusCodes.Status404NotFound.ToString());
            }

            return new EmptyResult();
        }
    }
}