using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;


namespace ExGoogleSheetsAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // 구글 클라우드 클라이언트 ID와 클라이언트 보안 비밀키 설정
            string clientId = "227948452553-bltojd7a935q6kgho4kr4fook1lidiqb.apps.googleusercontent.com";
            string clientSecret = "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";

            // 사용할 스프레드시트 ID와 시트 이름 설정
            string spreadsheetId = "1Hpbmpwl704afLj2vhuQ21IReuz42H6fX5SJMHBSZwJ4";
            string sheetName = "시트1";

            // 인증 정보 생성
            var credential = GoogleCredential.FromAccessToken(GetAccessToken(clientId, clientSecret));

            // Google Sheets API 클라이언트 생성
            var service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Sheets Test",
            });

            // 스프레드시트에서 시트 데이터 가져오기
            var range = $"{sheetName}!A1:B2";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();

            // 데이터 출력
            if (response != null && response.Values != null)
            {
                foreach (var row in response.Values)
                {
                    Console.WriteLine($"{row[0]}, {row[1]}");
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            Console.ReadLine();
        }

        // 구글 인증 정보 가져오기
        static string GetAccessToken(string clientId, string clientSecret)
        {
            string[] scopes = new[] { SheetsService.Scope.SpreadsheetsReadonly };
            var client = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(client, scopes, "user", System.Threading.CancellationToken.None).Result;
            return credential.Token.AccessToken;
        }
    }
}
