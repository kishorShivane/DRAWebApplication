using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using DRAWeb.Core.Interface;
using DRAWeb.Logger;
using DRAWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using static DRAWeb.App.Utilities.EnumHelpers;

namespace DRAWeb.App.Controllers
{
    public class ReportController : BaseController
    {
        IReportBroker reportService = null;
        public ReportController(IConfiguration _config, ILoggerManager loggerManager, IReportBroker service)
        {
            reportService = service;
            logger = loggerManager;
            config = _config;
        }

        public async Task<IActionResult> UserCompetency()
        {
            var user = GetUserSession();
            if (user == null)
            {
                return RedirectToAction("Login", "UserAccount");
            }
            else
            {
                if (user.IsTestTaken)
                {
                    List<UserCompetencyMatrixModel> lstMetrix = await GetUserCompetency();

                    if (lstMetrix == null)
                    {
                        SetNotification("Failed to execute the request!!", NotificationType.Failure, "Failed");
                    }
                    return View(lstMetrix);
                }
                return View();
            }
        }

        public async Task<List<UserCompetencyMatrixModel>> GetUserCompetency()
        {
            var user = GetUserSession();
            if (user != null)
            {
                var request = new CompetenciesReportRequest();
                request.UserID = user.UserID;
                var result = await reportService.GetUserCompetencyMetrix(request);
                if (result != null)
                {
                    if (result.Content != null)
                    {
                        return result.Content;
                    }
                }
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {

            //step1: create array to holder header labels
            string[] col_names = new string[]{
                "Type",
                "Main Group",
                "Sub Group",
                "Competency",
                "Required Level",
                "Current Level",
                "Gap"
            };

            //step2: create result byte array
            byte[] byteData;

            var result = await GetUserCompetency();
            if (result == null)
            {
                SetNotification("Failed to execute the request!!", NotificationType.Failure, "Failed");
                return View("UserCompetency");
            }

            //step3: create a new package using memory safe structure
            using (var package = new ExcelPackage())
            {
                //step4: create a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("final");

                //step5: fill in header row
                //worksheet.Cells[row,col].  {Style, Value}
                for (int i = 0; i < col_names.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Style.Font.Size = 14;  //font
                    worksheet.Cells[1, i + 1].Value = col_names[i];  //value
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true; //bold
                    //border the cell
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //set background color for each sell
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 243, 214));
                }


                int row = 2;
                //step6: loop through query result and fill in cells
                foreach (var item in result.ToList())
                {
                    for (int col = 1; col <= col_names.Length; col++)
                    {
                        worksheet.Cells[row, col].Style.Font.Size = 12;
                        //worksheet.Cells[row, col].Style.Font.Bold = true;
                        worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    //set row,column data
                    //worksheet.Cells[row, 1,].Merge
                    worksheet.Cells[row, 1].Value = item.Type;
                    worksheet.Cells[row, 2].Value = item.MainGroup;
                    worksheet.Cells[row, 3].Value = item.SubGroup;
                    worksheet.Cells[row, 4].Value = item.Competency;
                    worksheet.Cells[row, 5].Value = item.RequiredLevel;
                    worksheet.Cells[row, 6].Value = item.CurrentLevel;
                    worksheet.Cells[row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    switch (item.Gap)
                    {
                        case 0:
                            worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            break;
                        case 1:
                            worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                            break;
                        case 2:
                            worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            break;
                        default:
                            worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(Color.White);
                            break;
                    }
                    worksheet.Cells[row, 7].Value = item.Gap;


                    //toggle background color
                    //even row with ribbon style
                    if (row % 2 == 0)
                    {
                        for (int i = 1; i < col_names.Length; i++)
                        {
                            worksheet.Cells[row, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, i].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                    }
                    row++;
                }
                //step7: auto fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                //---------------------------------------------------------------------------------------------
                #region PieChart
                //create a WorkSheet
                //ExcelWorksheet worksheet1 = package.Workbook.Worksheets.Add("DummyChart");

                ////fill cell data with a loop, note that row and column indexes start at 1
                //Random rnd = new Random();
                //for (int i = 1; i <= 10; i++)
                //{
                //    worksheet1.Cells[1, i].Value = "Value " + i;
                //    worksheet1.Cells[2, i].Value = rnd.Next(5, 15);
                //}

                ////create a new piechart of type Pie3D
                //ExcelPieChart pieChart = worksheet1.Drawings.AddChart("pieChart", eChartType.Pie3D) as ExcelPieChart;

                ////set the title
                //pieChart.Title.Text = "PieChart Example";

                ////select the ranges for the pie. First the values, then the header range
                //pieChart.Series.Add(ExcelRange.GetAddress(2, 1, 2, 10), ExcelRange.GetAddress(1, 1, 1, 10));

                ////position of the legend
                //pieChart.Legend.Position = eLegendPosition.Bottom;

                ////show the percentages in the pie
                //pieChart.DataLabel.ShowPercent = true;

                ////size of the chart
                //pieChart.SetSize(500, 400);

                ////add the chart at cell C5
                //pieChart.SetPosition(4, 0, 2, 0);
                #endregion

                //----------------------------------------------------------------------------------------------------------
                #region Line Chart
                //ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("Dummy Line");

                ////fill cell data with a loop, note that row and column indexes start at 1
                //Random rnd1 = new Random();
                //for (int i = 2; i <= 11; i++)
                //{
                //    worksheet2.Cells[1, i].Value = "Value " + (i - 1);
                //    worksheet2.Cells[2, i].Value = rnd1.Next(5, 25);
                //    worksheet2.Cells[3, i].Value = rnd1.Next(5, 25);
                //}
                //worksheet2.Cells[2, 1].Value = "Age 1";
                //worksheet2.Cells[3, 1].Value = "Age 2";

                ////create a new piechart of type Line
                //ExcelLineChart lineChart = worksheet2.Drawings.AddChart("lineChart", eChartType.Line) as ExcelLineChart;

                ////set the title
                //lineChart.Title.Text = "LineChart Example";

                ////create the ranges for the chart
                //var rangeLabel = worksheet2.Cells["B1:K1"];
                //var range1 = worksheet2.Cells["B2:K2"];
                //var range2 = worksheet2.Cells["B3:K3"];

                ////add the ranges to the chart
                //lineChart.Series.Add(range1, rangeLabel);
                //lineChart.Series.Add(range2, rangeLabel);

                ////set the names of the legend
                //lineChart.Series[0].Header = worksheet.Cells["A2"].Value.ToString();
                //lineChart.Series[1].Header = worksheet.Cells["A3"].Value.ToString();

                ////position of the legend
                //lineChart.Legend.Position = eLegendPosition.Right;

                ////size of the chart
                //lineChart.SetSize(700, 300);

                ////add the chart at cell B6
                //lineChart.SetPosition(5, 0, 1, 0);
                #endregion

                //------------------------------------------------------------------------------------------------

                List<ColumnChart> chartData = new List<ColumnChart>();


                var grpType = result.GroupBy(x => x.Type);

                foreach (var grp in grpType)
                {
                    chartData.Add(new ColumnChart()
                    {
                        Type = grp.Key,
                        RequiredLevel = grp.Average(x => x.RequiredLevel),
                        CurrentLevel = grp.Average(x => x.CurrentLevel)
                    });
                }

                if (chartData.Any())
                {
                    var ws = package.Workbook.Worksheets.Add("newsheet");
                    var count = chartData.Count;
                    //Some data
                    for (var i = 1; i <= chartData.Count; i++)
                    {
                        var item = chartData[i - 1];
                        var area = (i + 1 + 10).ToString();

                        ws.Cells["A" + area].Value = item.Type;
                        ws.Cells["B" + area].Value = item.RequiredLevel;
                        ws.Cells["C" + area].Value = item.CurrentLevel;
                    }


                    //Create the chart
                    var chartRequiredLevel = (ExcelBarChart)ws.Drawings.AddChart("RequiredLevel", eChartType.ColumnClustered);
                    chartRequiredLevel.SetSize(1000, 600);
                    chartRequiredLevel.SetPosition(10, 10);
                    chartRequiredLevel.Style = eChartStyle.Style10;
                    chartRequiredLevel.Title.Text = "User Competency Graph";
                    chartRequiredLevel.Series.Add(ExcelRange.GetAddress(12, 2, chartData.Count + 1 + 10, 2), ExcelRange.GetAddress(12, 1, chartData.Count + 1 + 10, 1)).Header = "Average of Required Level";
                    chartRequiredLevel.Series.Add(ExcelRange.GetAddress(12, 3, chartData.Count + 1 + 10, 3), ExcelRange.GetAddress(12, 1, chartData.Count + 1 + 10, 1)).Header = "Average of Current Level";
                    chartRequiredLevel.Legend.Position = eLegendPosition.Right;
                    chartRequiredLevel.VaryColors = true;
                }


                //step8: convert the package as byte array
                byteData = package.GetAsByteArray();
            }//end using

            //step9: return byte array as a file
            return File(byteData, "application/vnd.ms-excel", $"UserCompetency{DateTime.Now.ToString("MMddyyyy")}.xls");



        }//end fun
    }
    public class ColumnChart
    {
        public string Type { get; set; }
        public double RequiredLevel { get; set; }
        public double CurrentLevel { get; set; }
    }
}