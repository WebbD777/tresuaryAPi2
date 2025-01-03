

namespace tresuaryAPi2;

using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

using Microsoft.Maui.Devices;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public partial class Category : ContentPage
{
    string url = "https://municipaldata.treasury.gov.za/api/cubes/incexp/members/item";
    List<string> itemCode = new List<string>();
    List<string> itemLabel = new List<string>();
    string _itemCode;
    int _year;
    string _demarCOde;

    FunctionViewModel model = new FunctionViewModel();
    public Category(string code)
	{

        InitializeComponent();

        _demarCOde = code;

        GetCategories(url);
    }
    public async Task<List<List<string>>> GetCategories(string url)
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
                        if (item["item.label"] != null)
                        {
                            string itemCodestr = item["item.code"].ToString().Split(',')[0];
                            string itemLabelstr = count + " " + item["item.label"].ToString().Split(',')[0];

                            count++;
                            // displayInfo.Text = itemCode;

                            itemCode.Add(itemCodestr);
                            itemLabel.Add(itemLabelstr);

                            model.FunctionItems.Add(new FunctionItem { FunctionText = itemLabelstr });
                        }
                    }
                }

                BindingContext = model;

            //   categoryList.ItemsSource = itemLabel;
                /*List<String> arrears = new List<string>();
                arrears.Add("Hello");
                arrears.Add("Mate");
                MuniList.ItemsSource = names;

                labels.Add(codes);
                labels.Add(names);*/
            }
        }

        return labels;
    }

    // For selecting CoolectionView Item
    private void OnSelectionChanged2(object sender, SelectionChangedEventArgs e)
    {
        var slectedItem = e.CurrentSelection[0] as FunctionItem; ;
        submitBTN.Text = slectedItem.FunctionText;
    }

    // For pressing Button
    private void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        submitBTN.BackgroundColor = new Color(255, 4, 6);

        _itemCode = submitBTN.Text.Substring(0, 1);
        int intItemCode = int.Parse(_itemCode);

        string getYear = inputField.Text;
        _year = int.Parse(getYear);

        if (2024 > _year && _year > 2007)
        {
            movePage(_year, itemCode[intItemCode], itemLabel[intItemCode]);
        }
    }

    private async void movePage(int year, string codeStr, string label)
    {
        //await Navigation.PushAsync(new Page2(muniCode, codes));
        await Navigation.PushAsync(new outPutPage(year, codeStr, label, _demarCOde));
    }
}

class FunctionItem
{
    public string FunctionText { get; set; }
}

class FunctionViewModel
{
    public ObservableCollection<FunctionItem> FunctionItems { get; set; }

    public FunctionViewModel()
    {
        // Initialize with some sample data
        FunctionItems = new ObservableCollection<FunctionItem>();
    }
}