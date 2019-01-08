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

namespace PersonalAccountOfStudent.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        private IHostingEnvironment Env { get; }
        private FileInfo fileInfo;
        private string path;

        public ScheduleController(IHostingEnvironment env)
        {
            Env = env;
            path = Path.Combine(Env.WebRootPath, "files", "Schedule.xlsx");
            fileInfo = new FileInfo(path);
        }

        public IActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            if (System.IO.File.Exists(path) && fileInfo.Length > 0)
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var memory = new MemoryStream();
                    stream.CopyTo(memory);
                    memory.Position = 0;

                    ISheet sheet;
                    string fileExtension = fileInfo.Extension;
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(memory); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook xssfwb = new XSSFWorkbook(memory); //This will read 2007 Excel format  
                        sheet = xssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }

                    sb.Append("<table class='table'><tr>");

                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int i = 0; i < cellCount; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(i);
                        if (cell == null || String.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th colspan='2'>" + cell.ToString() + "</th>");
                    }

                    sb.Append("</tr>");

                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        sb.AppendLine("<tr>");

                        IRow row = sheet.GetRow(i);
                        if (row == null || row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j <= cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }

                        sb.AppendLine("</tr>");
                    }

                    sb.Append("</table>");
                }

                ViewData["FileExist"] = true;
                ViewData["Grid"] = sb.ToString();
            }
            else
            {
                ViewData["FileExist"] = false;
                ViewData["Grid"] = "";
            }

            return View();
        }

        public async Task<IActionResult> DownloadSchedule()
        {
            if (System.IO.File.Exists(path) && fileInfo.Length > 0)
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
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
    }
}