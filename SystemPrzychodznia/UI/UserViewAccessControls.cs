using System;
using System.Collections.Generic;
using System.Text;
using SystemPrzychodznia.Data;

namespace SystemPrzychodznia
{
    public partial class FormUserView : Form
    {
        public void grantTabsToCurrentUser()
        {
            bool isSuperAdmin = _currentUser.Uprawnienia.HasRole(PermissionRoles.SuperAdmin);
            bool isAdmin = _currentUser.Uprawnienia.HasRole(PermissionRoles.Admin);
            bool isDoctor = _currentUser.Uprawnienia.HasRole(PermissionRoles.Lekarz);
            bool isReception = _currentUser.Uprawnienia.HasRole(PermissionRoles.Recepcja);

            // Uprawnienie 1 - SuperAdmin
            if (isSuperAdmin)
            {
                if (!tabControlUserView.TabPages.Contains(tabPageAdminViewUsers))
                {
                    tabControlUserView.TabPages.Add(tabPageAdminViewUsers);
                    populateUprawnienia();
                    LoadUsers();
                }
                if (!tabControlUserView.TabPages.Contains(tabPageAdminViewForgotten))
                {
                    tabControlUserView.TabPages.Add(tabPageAdminViewForgotten);
                    LoadForgottenUsers();
                }
                if (!tabControlUserView.TabPages.Contains(tabPageRoles))
                {
                    tabControlUserView.TabPages.Add(tabPageRoles);
                    LoadRoles();
                }

                EnsurePatientModuleVisible();
                EnsureVisitModuleVisible();
                //EnsureDoctorVisitsModuleVisible();

            }

            // Uprawnienie 2 - Admin
            if (isAdmin)
            {
                if (!tabControlUserView.TabPages.Contains(tabPageAdminViewUsers))
                { 
                    tabControlUserView.TabPages.Add(tabPageAdminViewUsers);
                    populateUprawnienia();
                    LoadUsers();
                }
                if (!tabControlUserView.TabPages.Contains(tabPageAdminViewForgotten)) 
                { 
                    tabControlUserView.TabPages.Add(tabPageAdminViewForgotten);
                    LoadForgottenUsers();
                }
                if (!tabControlUserView.TabPages.Contains(tabPageRoles))
                {
                    tabControlUserView.TabPages.Add(tabPageRoles);
                    LoadRoles();
                }
            }

            // Uprawnienie 4 - Recepcja
            // Lekarz powinien korzystać wyłącznie z modułu "Twoje Wizyty" z modułu 5,
            // Natomiast możę być użytkownik z uprawnieniami Recepcja i Lekarz, wtedy będzie miał dostęp do obu modułów.
            if (isReception)
            {
                EnsurePatientModuleVisible();
                EnsureVisitModuleVisible();
            }

            // Uprawnienie 3 - Lekarz
            if (isDoctor)
            {
                EnsureDoctorVisitsModuleVisible();
            }


            // Każdy użytkownik ma dostęp do zakładki "O programie"
            tabControlUserView.TabPages.Add(tabPageAbout);
            LoadAbout();
        }
    }
}
