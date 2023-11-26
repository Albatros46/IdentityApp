using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityApp.TagHelpers
{
    [HtmlTargetElement("td",Attributes = "asp-role-users")]
    public class RoleUsersTagHelper:TagHelper
    {/*
      Roles/Index.csthml de o role sahip kullanicilari görüntüleyebilmek icin TagHelper kullanacagiz.
        TagHelper td etiketinde calisacak ve Attribute ise asp-role-users olacak.
      */
        private RoleManager<AppRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public RoleUsersTagHelper(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HtmlAttributeName("asp-role-users")]
        public string RoleId { get; set; }= null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var userNames=new List<string>();
            var role=await _roleManager.FindByIdAsync(RoleId);
            if (role != null && role.Name !=null)
            {
                foreach (var user in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(user,role.Name))
                    {
                        userNames.Add(user.UserName ?? "");
                    }
                }
             //   output.Content.SetContent(userNames.Count==0 ? "Kullanici Yok!": string.Join(",",userNames)); //diger yöntem
                output.Content.SetHtmlContent(userNames.Count==0 ? "Kullanici Yok!":setHtml(userNames));
            }
            //return base.ProcessAsync(context, output);
        }

        private string setHtml(List<string> userNames)
        {
            var html="<ul>";
            foreach (var item in userNames)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul>";
            return html;
        }
    }
}
