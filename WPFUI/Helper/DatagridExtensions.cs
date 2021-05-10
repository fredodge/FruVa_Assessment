using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFUI.Helper
{
    public static class DatagridExtensions
    {
        public static void ExportUsingRefection(this DataGrid grid, IExporter exporter, string exportPath)
        {
            if (grid.ItemsSource == null || grid.Items.Count.Equals(0))
                throw new InvalidOperationException("You cannot export any data from an empty DataGrid.");

            IEnumerable<DataGridColumn> columns = grid.Columns.OrderBy(c => c.DisplayIndex);
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(grid.ItemsSource);
            foreach (object o in collectionView)
            {
                if (o.Equals(CollectionView.NewItemPlaceholder))
                    continue;

                foreach (DataGridColumn column in columns)
                {
                    if (column is DataGridBoundColumn)
                    {
                        string propertyValue = string.Empty;

                        /* Get the property name from the column's binding */
                        BindingBase bb = (column as DataGridBoundColumn).Binding;
                        if (bb != null)
                        {
                            Binding binding = bb as Binding;
                            if (binding != null)
                            {
                                string boundProperty = binding.Path.Path;

                                /* Get the property value using reflection */
                                PropertyInfo pi = o.GetType().GetProperty(boundProperty);
                                if (pi != null)
                                {
                                    object value = pi.GetValue(o);
                                    if (value != null)
                                        propertyValue = value.ToString();
                                    else if (column is DataGridCheckBoxColumn)
                                        propertyValue = "-";
                                }
                            }
                        }

                        exporter.AddColumn(propertyValue);
                    }
                    else if (column is DataGridComboBoxColumn)
                    {
                        DataGridComboBoxColumn cmbColumn = column as DataGridComboBoxColumn;
                        string propertyValue = string.Empty;

                        /* Get the property name from the column's binding */
                        BindingBase bb = cmbColumn.SelectedValueBinding;
                        if (bb != null)
                        {
                            Binding binding = bb as Binding;
                            if (binding != null)
                            {
                                string boundProperty = binding.Path.Path; //returns "Category" (or CategoryId)

                                /* Get the selected property */
                                PropertyInfo pi = o.GetType().GetProperty(boundProperty);
                                if (pi != null)
                                {
                                    object boundProperyValue = pi.GetValue(o); //returns the selected Category object or CategoryId
                                    if (boundProperyValue != null)
                                    {
                                        Type propertyType = boundProperyValue.GetType();
                                        if (propertyType.IsPrimitive || propertyType.Equals(typeof(string)))
                                        {
                                            if (cmbColumn.ItemsSource != null)
                                            {
                                                /* Find the Category object in the ItemsSource of the ComboBox with
                                                 * an Id (SelectedValuePath) equal to the selected CategoryId */
                                                IEnumerable<object> comboBoxSource = cmbColumn.ItemsSource.Cast<object>();
                                                object obj = (from oo in comboBoxSource
                                                              let prop = oo.GetType().GetProperty(cmbColumn.SelectedValuePath)
                                                              where prop != null && prop.GetValue(oo).Equals(boundProperyValue)
                                                              select oo).FirstOrDefault();
                                                if (obj != null)
                                                {
                                                    /* Get the Name (DisplayMemberPath) of the Category object */
                                                    if (string.IsNullOrEmpty(cmbColumn.DisplayMemberPath))
                                                    {
                                                        propertyValue = obj.GetType().ToString();
                                                    }
                                                    else
                                                    {
                                                        PropertyInfo displayNameProperty = obj.GetType()
                                                            .GetProperty(cmbColumn.DisplayMemberPath);
                                                        if (displayNameProperty != null)
                                                        {
                                                            object displayName = displayNameProperty.GetValue(obj);
                                                            if (displayName != null)
                                                                propertyValue = displayName.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                /* Export the scalar property value of the selected object
                                                 * specified by the SelectedValuePath property of the DataGridComboBoxColumn */
                                                propertyValue = boundProperyValue.ToString();
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(cmbColumn.DisplayMemberPath))
                                        {
                                            /* Get the Name (DisplayMemberPath) property of the selected Category object */
                                            PropertyInfo pi2 = boundProperyValue.GetType()
                                                .GetProperty(cmbColumn.DisplayMemberPath);

                                            if (pi2 != null)
                                            {
                                                object displayName = pi2.GetValue(boundProperyValue);
                                                if (displayName != null)
                                                    propertyValue = displayName.ToString();
                                            }
                                        }
                                        else
                                        {
                                            propertyValue = o.GetType().ToString();
                                        }
                                    }
                                }
                            }
                        }

                        exporter.AddColumn(propertyValue);
                    }
                }
                exporter.AddLineBreak();
            }
            /* Create and open export file */
            Process.Start(exporter.Export(exportPath));
        }
    }
}
