using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Services.Facade.Extensions;
using Services.Dao.Implementations.Generics;

namespace UI.Helpers
{
    public static class FormHelpers
    {
        public static void ClearControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Clear();
                    continue;
                }

                if (ctrl is MaterialTextBox)
                {
                    ((MaterialTextBox)ctrl).Clear();
                    continue;
                }

                if (ctrl is ComboBox)
                {
                    ComboBox cb = ctrl as ComboBox;
                    cb.SelectedIndex = cb.Items.Count > 0 ? 0 : cb.SelectedIndex;
                    continue;
                }

                if (ctrl is CheckBox)
                {
                    ((CheckBox)ctrl).Checked = false;
                    continue;
                }

                if (ctrl is RadioButton)
                {
                    ((RadioButton)ctrl).Checked = false;
                    continue;
                }

                if (ctrl.HasChildren)
                {
                    ClearControls(ctrl);
                }
            }
        }

        public static Control FindControl(Control parent, string controlName)
        {
            foreach (Control child in parent.Controls)
            {
                if (child.Name == controlName)
                {
                    return child;
                }

                Control foundControl = FindControl(child, controlName);

                if (foundControl != null)
                {
                    return foundControl;
                }
            }
            return null;
        }

        public static DateTime ParseFecha(string fechaNacimientoText)
        {
            DateTime fechaNacimiento;

            DateTime.TryParseExact(fechaNacimientoText,
                                                    new[] { "ddMMyyyy", "dd/MM/yyyy" },
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out fechaNacimiento);

            return fechaNacimiento;
        }

        public static void RefreshControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                
                ctrl.Invalidate();  
                ctrl.Update();      
                ctrl.Refresh();     

                if (ctrl.HasChildren)
                {
                    RefreshControls(ctrl);
                }
            }
        }
        public static void LoadFormInTab(TabPage tabPage)
        {
            Form formToLoad = null;

            try
            {
                // Construir el nombre del formulario a partir del nombre de la pestaña
                //string formName = $"UI.NonProfessional.{tabPage.Name}Form";
                string formName = FindFormByName($"{tabPage.Name}Form");
                Type formType = Type.GetType($"{formName}");

                if (formType != null)
                {
                    formToLoad = (Form)Activator.CreateInstance(formType);
                    formToLoad.TopLevel = false;
                    formToLoad.FormBorderStyle = FormBorderStyle.None;
                    formToLoad.Dock = DockStyle.Fill;
                    tabPage.Controls.Add(formToLoad);
                    formToLoad.Show();
                }
                else
                {
                    MessageBox.Show($"{"Formulario".Translate()} {formName} {"no encontrado.".Translate()}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{"Error al cargar el formulario:".Translate()} {ex.Message}");
            }
        }

        private static string FindFormByName(string formName)
        {
            // Obtener todos los ensamblados cargados en el dominio de la aplicación
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Recorrer cada ensamblado y buscar el tipo que coincide con el nombre completo del formulario
            foreach (var assembly in assemblies)
            {
                if (assembly.IsDynamic)
                    continue;

                try
                {
                    var formType = assembly.GetTypes().FirstOrDefault(t =>
                        t.FullName != null && t.FullName.EndsWith(formName) && typeof(Form).IsAssignableFrom(t));

                    if (formType != null)
                    {
                        return formType.FullName;
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Manejar excepción si es necesario
                    Console.WriteLine(ex);
                }
            }

            // Si no encuentra el formulario, devolver null
            return null;
        }
        public static void TranslateControls(Form form)
        {
            // Llama al método recursivo con el control principal (formulario)
            foreach (Control control in form.Controls)
            {
                TranslateRecursive(control);
            }
        }
        private static void TranslateRecursive(Control control)
        {
            if (control == null || control.Text == null || !(control.Text is string)) return;

            if (control is DateTimePicker || 
                control is NumericUpDown  ||
                control is MaterialTextBox ||
                control is MaterialComboBox) return;

                // Si el control tiene un texto, lo traduce
                if (!string.IsNullOrEmpty(control.Text))
            {

                control.Text = control.Text.Translate();
            }

            // Manejo especial para MenuStrip
            if (control is MenuStrip menuStrip)
            {
                foreach (ToolStripItem item in menuStrip.Items)
                {
                    TranslateMenuItem(item);
                }
            }
            // Manejo especial para TabControl
            if (control is TabControl tabControl)
            {
                TranslateTabCtrl(tabControl);
            }
            // Si el control tiene hijos, los procesa recursivamente
            foreach (Control child in control.Controls)
            {
                TranslateRecursive(child);
            }
        }
        private static void TranslateMenuItem(ToolStripItem menuItem)
        {
            if (!string.IsNullOrEmpty(menuItem.Text))
            {
                menuItem.Text = menuItem.Text.Translate();
            }

            // Si el menú tiene sub-items (como un menú desplegable), procesarlos recursivamente
            if (menuItem is ToolStripMenuItem menu)
            {
                foreach (ToolStripItem subItem in menu.DropDownItems)
                {
                    TranslateMenuItem(subItem);
                }
            }
        }
        private static void TranslateTabCtrl(TabControl tabctrl)
        {
            // Manejo especial para TabControl
            foreach (TabPage tabPage in tabctrl.TabPages)
            {
                // Traducir el texto de la pestaña
                if (!string.IsNullOrEmpty(tabPage.Text))
                {

                    tabPage.Text = tabPage.Text.Translate();
                }

                // Traducir los controles dentro de la pestaña
                foreach (Control tabChild in tabPage.Controls)
                {
                    TranslateRecursive(tabChild);
                }
            }
        }
    }
}
