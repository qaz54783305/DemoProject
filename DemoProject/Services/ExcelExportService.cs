using DemoProject.Models.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace DemoProject.Services;

public class ExcelExportService
{
    public byte[] ExportProducts(List<Product> products)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("商品清單");

        // 標題列樣式
        SetHeaderStyle(sheet.Cells[1, 1, 1, 6]);
        sheet.Cells[1, 1].Value = "編號";
        sheet.Cells[1, 2].Value = "商品名稱";
        sheet.Cells[1, 3].Value = "品牌";
        sheet.Cells[1, 4].Value = "類別";
        sheet.Cells[1, 5].Value = "售價";
        sheet.Cells[1, 6].Value = "庫存數量";

        for (int i = 0; i < products.Count; i++)
        {
            int row = i + 2;
            var p = products[i];
            sheet.Cells[row, 1].Value = p.Id;
            sheet.Cells[row, 2].Value = p.Name;
            sheet.Cells[row, 3].Value = p.Brand;
            sheet.Cells[row, 4].Value = p.Category;
            sheet.Cells[row, 5].Value = p.UnitPrice;
            sheet.Cells[row, 5].Style.Numberformat.Format = "#,##0";
            sheet.Cells[row, 6].Value = p.Stock;

            if (i % 2 == 1)
                SetAlternateRowStyle(sheet.Cells[row, 1, row, 6]);
        }

        ApplyBorderAndAutoFit(sheet, products.Count, 6);
        return package.GetAsByteArray();
    }

    public byte[] ExportBudgets(List<Budget> budgets)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("預算報表");

        SetHeaderStyle(sheet.Cells[1, 1, 1, 7]);
        sheet.Cells[1, 1].Value = "品牌";
        sheet.Cells[1, 2].Value = "通路";
        sheet.Cells[1, 3].Value = "年份";
        sheet.Cells[1, 4].Value = "月份";
        sheet.Cells[1, 5].Value = "預算金額";
        sheet.Cells[1, 6].Value = "實際金額";
        sheet.Cells[1, 7].Value = "狀態";

        for (int i = 0; i < budgets.Count; i++)
        {
            int row = i + 2;
            var b = budgets[i];
            sheet.Cells[row, 1].Value = b.Brand;
            sheet.Cells[row, 2].Value = b.Channel;
            sheet.Cells[row, 3].Value = b.Year;
            sheet.Cells[row, 4].Value = b.Month;
            sheet.Cells[row, 5].Value = b.BudgetAmount;
            sheet.Cells[row, 5].Style.Numberformat.Format = "#,##0";
            sheet.Cells[row, 6].Value = b.ActualAmount;
            sheet.Cells[row, 6].Style.Numberformat.Format = "#,##0";
            sheet.Cells[row, 7].Value = b.Status;

            // 狀態欄位顏色
            var statusColor = b.Status switch
            {
                "Approved"  => Color.FromArgb(198, 239, 206), // 綠
                "Reviewing" => Color.FromArgb(255, 235, 156), // 黃
                _           => Color.FromArgb(255, 199, 206)  // 紅（Draft）
            };
            sheet.Cells[row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(statusColor);

            if (i % 2 == 1)
                SetAlternateRowStyle(sheet.Cells[row, 1, row, 6]);
        }

        ApplyBorderAndAutoFit(sheet, budgets.Count, 7);
        return package.GetAsByteArray();
    }

    private static void SetHeaderStyle(ExcelRange cells)
    {
        cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cells.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
        cells.Style.Font.Color.SetColor(Color.White);
        cells.Style.Font.Bold = true;
        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    }

    private static void SetAlternateRowStyle(ExcelRange cells)
    {
        cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cells.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
    }

    private static void ApplyBorderAndAutoFit(ExcelWorksheet sheet, int dataCount, int colCount)
    {
        var range = sheet.Cells[1, 1, dataCount + 1, colCount];
        range.AutoFitColumns();
        var border = range.Style.Border;
        border.Top.Style    = ExcelBorderStyle.Thin;
        border.Bottom.Style = ExcelBorderStyle.Thin;
        border.Left.Style   = ExcelBorderStyle.Thin;
        border.Right.Style  = ExcelBorderStyle.Thin;
    }
}
