using System.Collections.ObjectModel;

namespace tresuaryAPi2
{
    using Microsoft.Maui.Devices;
    using Newtonsoft.Json.Linq;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    public partial class MainPage : ContentPage
    {
        // int count = 0;
        List<string> codes = new List<string>();
        List<string> names = new List<string>();
        public string muniCode;

        MainViewModel model = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
            GetMunicipalityNames("https://municipaldata.treasury.gov.za/api/cubes/incexp/members/demarcation");
        }

        private void OnMuniClicked(object sender, EventArgs e)
        {
            MuniBtn.BackgroundColor = new Color(255,4,6);

            muniCode = MuniBtn.Text.Substring(0, 1);

            movePage();
            

        }

        private async void movePage()
        {
            //await Navigation.PushAsync(new Page2(muniCode, codes));
            await Navigation.PushAsync(new Category(codes[int.Parse(muniCode)]));
        }

        private readonly HttpClient _client = new HttpClient();

        public async Task<string> GetMunicipalityData(string url)
        {
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responeH = await response.Content.ReadAsStringAsync();
               // return responeH;
                Console.WriteLine(responeH);
               // displayInfo.Text = responeH;
                return responeH;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task<List<List<string>>> GetMunicipalityNames(string url)
        {
            
            List<List<string>> labels = new List<List<string>>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(responseData);

                    if (data["data"] != null)
                    {
                        int count = 0;
                        foreach (var item in data["data"])
                        {
                            if (item["demarcation.label"] != null)
                            {
                                string itemCode = item["demarcation.code"].ToString().Split(',')[0];
                                string itemName = count + " " + item["demarcation.label"].ToString().Split(',')[0];

                                count++;
                               // displayInfo.Text = itemCode;

                                codes.Add(itemCode);
                                names.Add(itemName);

                                model.MuniItems.Add(new MuniItem { LabelText = itemName});
                            }
                        }
                    }
                    List<String> arrears = new List<string>();
                    arrears.Add("Hello");
                    arrears.Add("Mate");
                  //  MuniList.ItemsSource = names;

                    
                    
                    labels.Add(codes);
                    labels.Add(names);

                   
                   
                }
            }

            BindingContext = model;

            return labels;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            var slectedItem = e.CurrentSelection[0] as MuniItem; ;

           // MuniList.BackgroundColor = new Color(100, 50, 6);

            MuniBtn.Text = slectedItem.LabelText;


        }

    }

}

class MuniItem
{
    public string LabelText { get; set; }
}

class MainViewModel
{
    public ObservableCollection<MuniItem> MuniItems { get; set; }

    public MainViewModel()
    {
        // Initialize with some sample data
        MuniItems = new ObservableCollection<MuniItem>();
    }

}
