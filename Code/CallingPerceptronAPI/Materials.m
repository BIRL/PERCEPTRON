
json = '[{"Title":"Default Run","FDRCutOff":"0.0","ProteinDatabase":"Human","FileName":"C:\\fakepath\\HELA_pk13_sw1_66sc_mono.txt","NumberOfOutputs":"100+","MassMode":"M(Neutral)","FilterDb":true,"MwTolerance":500,"PeptideTolerance":15,"PeptideToleranceUnit":"ppm","Autotune":false,"InsilicoFragType":"HCD","HandleIons":"bo,bstar,yo,ystar","DenovoAllow":true,"MinimumPstLength":"3","MaximumPstLength":6,"HopThreshhold":0.1,"Hop_Tolerance_Unit":"Da","PSTTolerance":0.45,"Truncation":false,"TerminalModification":"None,NME,NME_Acetylation,M_Acetylation","PtmAllow":false,"PtmTolerance":0.5,"List_of_Modifications":"","Variable_Modifications":"","MethionineChemicalModification":"None","Fixed_Modification":"","CysteineChemicalModification":"None","MwSweight":"0","PstSweight":"0","InsilicoSweight":"100","VariableModifications":"","FixedModifications":"","EmailId":"farhan.khalid@lums.edu.pk","UserId":"farhan.khalid@lums.edu.pk","FDR_CutOff":"0.0"}]'

% % % import matlab.net.http.io.FileProvider.*
% % % import matlab.net.http.*
% % % import matlab.net.http.field.*
% % % import matlab.net.http.io.MultipartProvider.*
% % % import matlab.net.http.io.*
% % %import matlab.net.http.io.FileProvider.*
% json = "ok";
filepath = "D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\CallingPerceptronAPI\HELA_pk13_sw1_66sc_mono.txt"
uri = 'http://localhost:52340/api/search/Calling_API';
url = 'http://localhost:52340/api/search/Calling_API';

import matlab.net.http.*
import matlab.net.http.field.*
import matlab.net.http.io.*

request = RequestMessage(PUT,[],provider);
[response,completedrequest,history] = send(request,uri)

provider = FileProvider('dir/imageFile.jpg');
req = RequestMessage(PUT,[],provider);
resp = req.send(url);

url = "www.somewebsite.com";
provider = MultipartProvider(FileProvider(["image1.jpg", "file1.txt"]));
req = RequestMessage(PUT, [], provider);
resp = req.send(url);


import matlab.net.http.*
import matlab.net.http.field.*
request = RequestMessage( 'POST', ...
    [ContentTypeField( 'application/vnd.api+json' ), AcceptField('application/vnd.api+json')], json) %...
%    '{"meta": {"key": "xxxxxx"}}' );

[completedrequest,target] = complete(request,uri)
response = request.send( 'http://localhost:52340/api/search/Calling_API')



request = RequestMessage( 'POST', ...
    [ContentTypeField( 'application/x-www-form-urlencoded' ), AcceptField('application/json') ]  )  %vnd.api+
%, AcceptField('application/vnd.api+json')]
    %'{"meta": {"key": "xxxxxx"}}' );
    HTTPOptions.Timeout = 100000;
response = request.send( 'http://localhost:52340/api/search/Calling_API');


req = RequestMessage('POST',[],json);
req.send('http://localhost:52340/api/search/Calling_API');


data = json;
% body = matlab.net.http.MessageBody(data);
% body.show

uri = 'http://localhost:52340/api/search/Calling_API'

ContentType = 'application/json'
weboptions('RequestMethod','POST','ContentType', 'json','Timeout',60,'KeyName','Authorization','KeyValue','abc230')
% import matlab.net.http.*
% import matlab.net.http.field.*
% import matlab.net.http.io.*
% 
% provider = MultipartFormProvider("files", FileProvider(["dir/HELA_pk13_sw1_66sc_mono.txt", "dir/HELA_pk13_sw1_66sc_mono.txt"]),...
%     "text", FileProvider("dir/HELA_pk13_sw1_66sc_mono.txt"));
% provider = MultipartProvider(FileProvider(["dir/HELA_pk13_sw1_66sc_mono.txt", "dir/HELA_pk13_sw1_66sc_mono.txt"]));
% provider = FileProvider('dir/HELA_pk13_sw1_66sc_mono.txt');



% D:/01_PERCEPTRON/gitHub/PERCEPTRON/Code/CallingPerceptronAPI/HELA_pk13_sw1_66sc_mono.txt
% providers = FileProvider([filepath])
% fps = FileProvider(filepath); % get array of providers
% mp = MultipartProvider(fps);
% formProvider = MultipartFormProvider("submit-name","Larry","files",mp);
import matlab.net.http.io.*
import matlab.net.http.io.ContentProvider.*

headerField = matlab.net.http.field.ContentTypeField('application/x-www-form-urlencoded');
un = 'me';
pw = 'verySecret';
uri = 'http://localhost:52340/api/search/Calling_API'
input = struct('Username',un,'Password',pw, 'grant_type', 'password');
inputParameters = struct('parameters', input);
% aTest = jsonencode(inputParameters);
options = matlab.net.http.HTTPOptions();
method = matlab.net.http.RequestMethod.POST;
request = matlab.net.http.RequestMessage(method,headerField, json);

formProvider = MultipartFormProvider("submit-name","Larry","files",MultipartProvider(FileProvider(["dir/HELA_pk13_sw1_66sc_mono.txt"])));
show(request)
resp = send(request,uri, options);





fps = matlab.net.http.io.MultipartProvider.FileProvider(["HELA_pk13_sw1_66sc_mono.txt", "HELA_pk13_sw1_66sc_mono.txt"]); % get array of providers
mp = matlab.net.http.io.MultipartProvider(fps);
formProvider = matlab.net.http.io.MultipartProvider("submit-name","Larry","files",mp);
req = RequestMessage('put',[],formProvider);
req.send(uri);






%%%%%%%%




import matlab.net.http.*
import matlab.net.http.field.*
request = RequestMessage( 'POST', ...
    [ContentTypeField( 'application/vnd.api+json' ), AcceptField('application/vnd.api+json')], ...
    '{"meta": {"key": "xxxxxx"}}' );
% weboptions.HeaderFields = json;

HTTPOptions.Timeout = 100000;
response = request.send( 'http://localhost:52340/api/search/Calling_API' );


formProvider = json;


%%%%%%%%%%
complete(provider,URI)

%%contentTypeField = matlab.net.http.field.ContentTypeField('text/json');




S = worldBankTemps('USA')




function temperatures = worldBankTemps(country,options)
% Get World Bank temperatures for a country, for example, 'USA'.


api = 'https://perceptron.lums.edu.pk/index.html#/resultsdownload/754e2af5-c888-414f-ac69-412f04176eda';
%api = [api 'country/cru/tas/year/'];
country = [api country];


% api = 'http://climatedataapi.worldbank.org/climateweb/rest/v1/';
% api = [api 'country/cru/tas/year/'];
% country = [api country];

% The options object contains additional HTTP
% request parameters. If worldBankTemps was
% not passed options as an input argument,
% create a default weboptions object.
if ~exist('options','var')
    options = weboptions;
end
s = webread(country,options);

% Convert data to arrays
temperatures = struct('Years',[],'DegreesInFahrenheit',[]);
temperatures(1).Years = [s.year];
temperatures(1).DegreesInFahrenheit = [s.data];

% Convert temperatures to Fahrenheit
temperatures(1).DegreesInFahrenheit = temperatures(1).DegreesInFahrenheit * 9/5 + 32;
end