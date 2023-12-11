using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Badge.Pages.CelinasShitCorner
{
    public class dragdropModel : PageModel
    {
        public List<ticketitem> Ids { get; set; }
        ticketitem one = new ticketitem("1", null);
        ticketitem two = new ticketitem("2", null);

        public dragdropModel()
        {
            Ids= new List<ticketitem>();
            Ids.Add(one);
            Ids.Add(two);
        }

        public async Task OnGetAsync()
        {

        }

        public void dropGreen(string item)
        {
            ticketitem Item = Ids.FirstOrDefault(i => i.id == item);
            Item.title = "green";
        }

        public void dropBlue(string item)
        {
            ticketitem Item = Ids.FirstOrDefault(i => i.id == item);
            Item.title = "blue";
        }


    }

    public class ticketitem
    {
        public ticketitem(string id, string? title)
        {
            this.id=id;
            this.title=title;
        }

        public string id { get; set; }
        public string? title { get; set; }
    }
}
