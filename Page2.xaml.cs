//using ThreadNetwork;

namespace tresuaryAPi2;

using Microsoft.Maui.Devices;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
public partial class Page2 : ContentPage
{
	string muniCOde;
    private readonly HttpClient _client = new HttpClient();
    public Page2(string code, List<string> codesLs)
	{
        
		InitializeComponent();

        muniCOde = code;

        int codeInt = int.Parse(code);
        string municodeFromList = codesLs[codeInt];

        string url = $"https://municipaldata.treasury.gov.za/api/cubes/incexp/aggregate?drilldown=item.code|item.label|item.return_form_structure&cut=financial_year_end.year:2015|amount_type.code:AUDA|financial_period.period:2015|demarcation.code:\"{municodeFromList}\"&aggregates=amount.sum";

        GetMunicipalityData(url);

    }

    public async Task<string> GetMunicipalityData(string url)
    {

        var response = await _client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responeH = await response.Content.ReadAsStringAsync();
            // return responeH;
            Console.WriteLine(responeH);
            // displayInfo.Text = responeH;
            outputLabel.Text = responeH;
            return responeH;
        }
        else
        {
            throw new Exception($"Error: {response.StatusCode}");
        }
    }
}