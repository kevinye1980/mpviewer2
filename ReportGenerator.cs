using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.EnterpriseManagement.Configuration;
using Common;

namespace MPViewer
{
    class ReportGenerator
    {
        DataSet m_dataset;
        IList<ManagementPack> m_mp;

        //---------------------------------------------------------------------
        internal ReportGenerator(DataSet dataSet, IList<ManagementPack> mp)
        {
            m_dataset = dataSet;
            m_mp = mp;
        }

        //---------------------------------------------------------------------
        internal void GenerateHTMLReport(string filePath, bool exportAlertGeneratingWorkflowsOnly)
        {
            StringBuilder html = new StringBuilder("<HTML><Body>");
            foreach (ManagementPack MP in m_mp)
            {
                html.Append("<hr />");
                html.AppendFormat("<h3>Management Pack Name: {0}</h3>", Utilities.GetBestManagementPackName(MP));
                html.AppendFormat("<h3>Management Pack Version: {0}</h3>", MP.Version.ToString());
                html.Append("<hr />");
            }
            foreach (DataTable table in m_dataset.Tables)
            {
                if (table.Rows.Count > 0)
                {
                    table.DefaultView.Sort = "Name";

                    DataTable sortedTable = table.DefaultView.ToTable();

                    if (exportAlertGeneratingWorkflowsOnly && (table.TableName != "Monitors" && table.TableName != "Rules"))
                    {
                        continue;
                    }

                    html.AppendFormat(@"<h2 style=""color: #91723f;font-size:140%;font-family:Tahoma, Arial, Helvetica;font-weight:bold;padding: 15px 0px 15px 8px;margin:0px; "">{0}</h2>", table.TableName);

                    html.Append(GenerateHTMLTable(sortedTable, exportAlertGeneratingWorkflowsOnly));
                }
            }

            html.Append("</Body></HTML>");

            File.WriteAllText(filePath, html.ToString());

        }

        //---------------------------------------------------------------------
        private string GenerateHTMLTable(DataTable table, bool exportAlertGeneratingWorkflowsOnly)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(@"<table cellSpacing=""0"" cellPadding=""0"" border=""0"" width=""100%"">");
            stringBuilder.AppendLine("<thead>");
            stringBuilder.AppendLine("<tr>");
            IList<string> Columns = new List<string>();

            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName == "ObjectRef")
                {
                    continue;
                }
                Columns.Add(column.ColumnName);
                stringBuilder.AppendFormat(@"<th style=""text-align:left;padding:4px;border-style:solid;border-width:1px;border-left-width:0px;border-color: #eeeeee;background-color: #666666;color: #ffffff;font-family: Tahoma, Arial, Helvetica;font-size: 80%;font-weight: bold;"">{0}</th>", column.ColumnName);
            }

            stringBuilder.AppendLine("</tr>");
            stringBuilder.AppendLine("</thead>");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if ((i % 2) == 0)
                {
                    stringBuilder.AppendLine(@"<tr style=""padding:4px;border-style:solid;border-width:1px;border-top-width:0px;border-left-width:0px;border-color:#eeeeee;color:#333333;font-family: Tahoma, Arial, Helvetica;font-size:80%;text-align: left; "">");
                }
                else
                {
                    stringBuilder.AppendLine(@"<tr style=""padding:4px;border-style:solid;border-width:1px;border-top-width:0px;border-left-width:0px;border-color:#eeeeee;color:#333333;background-color: #eeeeee;font-family: Tahoma, Arial, Helvetica;font-size:80%;text-align: left; "">");
                }

                if (!IsAlertGeneratingWorkflow(table.Rows[i]) && exportAlertGeneratingWorkflowsOnly)
                {
                    continue;
                }

                foreach (string ColumnName in Columns)
                {
                    string rowText = table.Rows[i][ColumnName].ToString();

                    if (rowText == null || rowText.Length == 0)
                    {
                        rowText = " ";
                    }

                    if (table.Columns[ColumnName].ColumnName == "Name")
                    {
                        stringBuilder.AppendFormat(@"<td width=""300px"">{0}</td>", rowText);
                    }
                    else if (table.Columns[ColumnName].ColumnName == "Category")
                    {
                        stringBuilder.AppendFormat(@"<td width=""120px"">{0}</td>", rowText);
                    }
                    else if (table.Columns[ColumnName].ColumnName == "Enabled")
                    {
                        stringBuilder.AppendFormat(@"<td width=""75px"">{0}</td>", rowText);
                    }
                    else if (table.Columns[ColumnName].ColumnName == "Generate Alert")
                    {
                        stringBuilder.AppendFormat(@"<td width=""100px"">{0}</td>", rowText);
                    }
                    else if (table.Columns[ColumnName].ColumnName == "Alert Severity")
                    {
                        stringBuilder.AppendFormat(@"<td width=""100px"">{0}</td>", rowText);
                    }
                    else if (table.Columns[ColumnName].ColumnName == "Auto Resolve")
                    {
                        stringBuilder.AppendFormat(@"<td width=""100px"">{0}</td>", rowText);
                    }
                    else
                    {
                        stringBuilder.AppendFormat("<td>{0}</td>", rowText);
                    }
                }

                stringBuilder.AppendLine("</tr>");
            }

            stringBuilder.AppendLine("</table>");

            return (stringBuilder.ToString());
        }

        //---------------------------------------------------------------------
        internal void GenerateExcelReport(string filePath)
        {
            // some people type xls, we give them excel xml anyway...
            if (filePath.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
                filePath = filePath.Replace(".xml", ".xls");

            ExportToExcel(filePath, m_dataset);
        }

        // this version to be called in batch export, since dataset gets loaded somewhere else
        internal void ExportToExcel(string filename, DataSet dataset)
        {
            ExcelWriter writer = new ExcelWriter();

            foreach (DataTable table in dataset.Tables)
            {
                writer.WriteDataTable(table);
            }

            writer.SaveToFile(filename);
        }


        //---------------------------------------------------------------------
        private bool IsAlertGeneratingWorkflow(DataRow dataRow)
        {
            bool isAlertGeneratingWorkflow = false;

            try
            {
                bool containsGenerateAlertColumn;
                bool currentRuleGeneratesAlert;

                containsGenerateAlertColumn = dataRow.Table.Columns.Contains("Generate Alert");

                if (containsGenerateAlertColumn)
                {
                    currentRuleGeneratesAlert = ((bool)dataRow["Generate Alert"] == true);
                }

                isAlertGeneratingWorkflow = (containsGenerateAlertColumn && containsGenerateAlertColumn);
            }
            catch (Exception)
            {
            }

            return (isAlertGeneratingWorkflow);
        }
    }
}
