using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPrzychodznia
{
    public partial class FormUserView : Form
    {
        public void grantTabsToCurrentUser()
        {

            // Uprawnienie 1 - SuperAdmin
            if (_currentUser.Uprawnienia.Exists(u => u.Id == 1 && u.Posiadane == true))
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

            // Uprawnienie 2 - Admin
            if (_currentUser.Uprawnienia.Exists(u => u.Id == 2 && u.Posiadane == true))
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


            // Każdy użytkownik ma dostęp do zakładki "O programie"
            tabControlUserView.TabPages.Add(tabPageAbout);
        }
    }
}
