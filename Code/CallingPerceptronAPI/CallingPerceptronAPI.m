
    ftpobj = ftp('*****', '****', '****');
    %%Password Added here....
UniqueUserID = string(java.util.UUID.randomUUID.toString);   %% Generate Unique User ID

%%% ATTENTION!!! ALL PARAMETER VALUES ARE CASE SENSITIVE
%%% User will add values below...!!!
%Title
JobName = "Default Run";
%FDRCutOff
FDRCutOff = "0.0"; % User can add values from 0-100
%ProteinDatabase : There are two Protein Databases in Perceptron Human & Ecoli
ProteinDatabase = "Human"; % User can also, select "Ecoli" as well 

%%%%%
%REMINDER
%%% ATTENTION!!! ALL PARAMETER VALUES ARE CASE SENSITIVE
%REMINDER
%%%%%

%MassMode
MassMode = "M(Neutral)";  % User can also, select MH+
%FilterDb
FilterDb = "True";     % User can also, select "False"

%%% User will add values above...!!!
%%% ATTENTION!!! ALL PARAMETER VALUES ARE CASE SENSITIVE

%%% User Can Enter Inputs

% ParameterName = {'Title';'FDRCutOff';'ProteinDatabase';'MassMode';'FilterDb';'MwTolerance';'PeptideTolerance';'PeptideToleranceUnit';'Autotune';'InsilicoFragType';'HandleIons';'DenovoAllow';"MinimumPstLength"; "MaximumPstLength"; "HopThreshhold"; "Hop_Tolerance_Unit"; "PSTTolerance";'Truncation';'TerminalModification';'PtmAllow';'PtmTolerance';'List_of_Modifications'; 'Variable_Modifications';'MethionineChemicalModification';'Fixed_Modification'; 'CysteineChemicalModification'; 'MwSweight'; 'PstSweight'; 'InsilicoSweight'; 'VariableModifications';'FixedModifications';'EmailId';'UserId'};
% ParameterValue = [JobName;FDRCutOff;ProteinDatabase;MassMode;FilterDb;"500";"15";"ppm";"False";"HCD";"bo,bstar,yo,ystar";"True";"3";"6";"0.1";"Da";"0.45";"False"; "None,NME,NME_Acetylation,M_Acetylation";"False";"0.5" ; "";"";"None";"";"None";"0"; "0";"100";"";"";"farhan.khalid@lums.edu.pk"; "farhan.khalid@lums.edu.pk"];
% JsonString = jsonencode(table(ParameterName,ParameterValue))
% SendParameters.Body = JsonString;

FullFileName = 'D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\CallingPerceptronAPI\HELA_pk13_sw1_66sc_mono.txt'; % Please add here the path alongwith filename
[FilePath,FileName,Extension] = fileparts(FullFileName);
FileNameWithExtension = strcat(FileName,Extension);
try
    mput(ftpobj,FullFileName);
catch
    msgbox('Input file couldn''t uploaded at Server please check your internet connection and data file. If problem still persists report bug on GitHub','CallingPerceptronApi','Modal');
end

import matlab.net.http.*
%JUST CALL CHECKING...
SendParameters = RequestMessage('POST',[]);
Options = matlab.net.http.HTTPOptions('ConnectTimeout',1000);  %% 1000sec
BaseApiUrl = "http://localhost:52340/";
PerceptronApiJobSubmissionUrl = strcat(BaseApiUrl,'api/search/Calling_API');

Pattren = ":";

ParameterValue = strcat(JobName,Pattren,FDRCutOff,Pattren,ProteinDatabase,Pattren,MassMode,Pattren,FilterDb,Pattren,"500",Pattren,"15",Pattren,"ppm",Pattren,"False",Pattren,"HCD",Pattren,"bo,bstar,yo,ystar",Pattren,"True",Pattren,"3",Pattren,"6",Pattren,"0.1",Pattren,"Da",Pattren,"0.45",Pattren,"False",Pattren,"None,NME,NME_Acetylation,M_Acetylation",Pattren,"False",Pattren,"0.5",Pattren,"",Pattren,"",Pattren,"None",Pattren,"None",Pattren,"0",Pattren,"0",Pattren,"100",Pattren,"farhan.khalid@lums.edu.pk",Pattren,UniqueUserID,Pattren,FileNameWithExtension);
SendParameters.Body = ParameterValue;
Response = SendParameters.send(PerceptronApiJobSubmissionUrl, Options);
try
if (Response.StatusCode == "OK")
    java.lang.Thread.sleep(600);  %in sec
    PerceptronApiJobStatusUrl = strcat(BaseApiUrl,'api/search/CallingPerceptronApiHistory');
    JobStatusResquest = RequestMessage('POST',[]);
    JobStatusResquest.Body = UniqueUserID;
    JobStatusReponse = JobStatusResquest.send(PerceptronApiJobStatusUrl, Options);
    
    if (JobStatusReponse.Body.Data.progress == "In Queue" || JobStatusReponse.Body.Data.progress == "Running")
        Wait = true;
        while(Wait)
            java.lang.Thread.sleep(600);  %in sec
            JobStatusReponse = JobStatusResquest.send(PerceptronApiJobStatusUrl, Options);
            if (JobStatusReponse.Body.Data.progress == "Completed" || JobStatusReponse.Body.Data.progress == "Error in Query" || JobStatusReponse.Body.Data.progress == "Result Expired")
                Wait = false;
            end
        end
    end
    
    if (JobStatusReponse.Body.Data.progress == "Completed")
        
        
    elseif (JobStatusReponse.Body.Data.progress == "Error in Query")
        msgbox('Dear User, there is an error in your query.','CallingPerceptronApi','Modal');
        throw;
    elseif (JobStatusReponse.Body.Data.progress == "Result Expired")
        msgbox('Dear User, your search results has been expired.','CallingPerceptronApi','Modal');
        throw;
    
    end
    
    
    
    sdadsad= 1;
    
    
else
    msgbox('Search couldn''t complete please check your search parameters and data file. If problem still persists report bug on GitHub','CallingPerceptronApi','Modal');
end
catch
    
end

% Message = char(strcat('Dear User, please save this ID:', UniqueUserID, ' to access your results upto 48hrs.'));
% msgbox(Message,'CallingPerceptronApi','Modal');

fdsf = 23;





