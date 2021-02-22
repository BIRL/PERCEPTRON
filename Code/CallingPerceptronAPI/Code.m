import matlab.net.http.*





UrlCallToGenerateLink = 'http://localhost:52340/api/search/CallingToGenerateOneDriveLink';

LinkToUploadInputFile = 'https://pern-my.sharepoint.com/:f:/g/personal/perceptron_lums_edu_pk/EqHbhV5AsURIucXhZtY6sbEBptIF7g5GiwOhW8b_Lg8cXg'



dataFile = 'HELA_pk13_sw1_66sc_mono.txt';

output = uploadToDropbox(dropboxAccessToken,varargin)




ftpobj = ftpMatlabAlteredFunction(LinkToUploadInputFile);
mput(ftpobj,'HELA_pk13_sw1_66sc_mono.txt');


RequestToGenerateLink = RequestMessage('POST',[]);
Response = RequestToGenerateLink.send(UrlCallToGenerateLink);

LinkToUploadInputFile = Response.Body.Data;




% url = 'http://localhost:52340/api/search/Calling_API';
import matlab.net.http.io.StringProvider
import matlab.net.http.io.*
Name = {'Jones';'Brown'};
Age = [40;49];
json = jsonencode(table(Name,Age))
% parmeters = '[{"Title":"Default Run","FDRCutOff":"0.0","ProteinDatabase":"Human","FileName":"C:\\fakepath\\HELA_pk13_sw1_66sc_mono.txt","NumberOfOutputs":"100+","MassMode":"M(Neutral)","FilterDb":true,"MwTolerance":500,"PeptideTolerance":15,"PeptideToleranceUnit":"ppm","Autotune":false,"InsilicoFragType":"HCD","HandleIons":"bo,bstar,yo,ystar","DenovoAllow":true,"MinimumPstLength":"3","MaximumPstLength":6,"HopThreshhold":0.1,"Hop_Tolerance_Unit":"Da","PSTTolerance":0.45,"Truncation":false,"TerminalModification":"None,NME,NME_Acetylation,M_Acetylation","PtmAllow":false,"PtmTolerance":0.5,"List_of_Modifications":"","Variable_Modifications":"","MethionineChemicalModification":"None","Fixed_Modification":"","CysteineChemicalModification":"None","MwSweight":"0","PstSweight":"0","InsilicoSweight":"100","VariableModifications":"","FixedModifications":"","EmailId":"farhan.khalid@lums.edu.pk","UserId":"farhan.khalid@lums.edu.pk","FDR_CutOff":"0.0"}]';


fps = FileProvider("HELA_pk13_sw1_66sc_mono.txt");
fps = FileProvider("HELA_pk13_sw1_66sc_mono.zip");
mp = MultipartProvider(fps);
formProvider = MultipartFormProvider("Parameters",string(json),"InputFiles",mp);

% fps = FileProvider("HELA_pk13_sw1_66sc_mono.zip"); % get array of providers





% matlab.net.http.MessageBody(string(json));
% fps.Request.Body = string(json);
% headerField = matlab.net.http.field.ContentTypeField('application/json');
req = RequestMessage('POST',[],formProvider);
req.send(url);

provider = JSONProvider(json)










fds = 1
