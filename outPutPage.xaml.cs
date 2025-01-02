using Newtonsoft.Json.Linq;

namespace tresuaryAPi2;

using Microsoft.Maui.Devices;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
public partial class outPutPage : ContentPage
{
    List<string> sum = new List<string>();
    List<string> functionLabel = new List<string>();
    List<string> outPutLs = new List<string>();
    public outPutPage()
	{
		InitializeComponent();
	}

    public outPutPage(int year, string itemCode, string label, string demarCode)
    {
        InitializeComponent();

        //outPutLabel.Text = year + " " + itemCode + " " + label + " " + demarCode;
        string url = $"https://municipaldata.treasury.gov.za/api/cubes/incexp/aggregate?drilldown=function.label|item.return_form_structure&cut=financial_year_end.year:{year}|amount_type.code:AUDA|financial_period.period:{year}|demarcation.code:\"{demarCode}\"|item.code:\"{itemCode}\"&aggregates=amount.sum";
        getOutput(url);
    }

    private async Task<List<List<string>>> getOutput(string url)
    {

        List<List<string>> labels = new List<List<string>>();

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
              //  outPutLabel.Text = responseData;
                JObject data = JObject.Parse(responseData);
                //outPutLabel.Text = data["total_cell_count"].Value<int>().ToString();

                if (data["cells"] != null)
                {
                    outPutLabel.Text = "data not null";
                    int count = 0;

                    foreach (var item in data["cells"])
                    {
                        if (item["function.label"] != null)
                        {
                            string sumStr = item["amount.sum"].ToString().Split(',')[0];
                            string functionStr =item["function.label"].ToString().Split(',')[0];

                            count++;
                            // displayInfo.Text = itemCode;

                            string outPutStr = functionStr + "\nTotal amount: R" + sumStr;
                            outPutLs.Add(outPutStr);
                            // sum.Add(sumStr);
                            // functionLabel.Add(functionStr);
                        }
                    }
                }
                else
                {
                    //outPutLabel.Text = "data null";
                }

                lsView.ItemsSource = outPutLs;

                //  outPutLabel.Text = "Did soemthing";
                /*List<String> arrears = new List<string>();
                arrears.Add("Hello");
                arrears.Add("Mate");
                MuniList.ItemsSource = names;

                labels.Add(codes);
                labels.Add(names);*/
            }
            else {
                outPutLabel.Text = "Unsuccessful response";
            }
        }

        return labels;
    }
}