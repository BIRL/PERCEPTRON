
% UniqueID = java.util.UUID.randomUUID.toString;   %% Generate Unique ID so that it would be easy to work around
try
    ftpobj = ftp('*****', '****', '****');
    %%Password Added here....
    
    
    mput(ftpobj,"HELA_pk13_sw1_66sc_mono.txt");
    
    
    
catch
    msgbox('Search couldn''t complete please check your search parameters and data file. If problem still persists report bug on GitHub','CallingPerceptronApi','Modal');
end

import matlab.net.http.*

PerceptronApiUrl = 'http://localhost:52340/api/search/Calling_API';

%JUST CALL CHECKING...
SendParameters = RequestMessage('POST',[]);
Options = matlab.net.http.HTTPOptions('ConnectTimeout',1000);  %% 1000s
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
Pattren = ":";



ParameterValue = strcat(JobName,Pattren,FDRCutOff,Pattren,ProteinDatabase,Pattren,MassMode,Pattren,FilterDb,Pattren,"500",Pattren,"15",Pattren,"ppm",Pattren,"False",Pattren,"HCD",Pattren,"bo,bstar,yo,ystar",Pattren,"True",Pattren,"3",Pattren,"6",Pattren,"0.1",Pattren,"Da",Pattren,"0.45",Pattren,"False",Pattren,"None,NME,NME_Acetylation,M_Acetylation",Pattren,"False",Pattren,"0.5",Pattren,"",Pattren,"",Pattren,"None",Pattren,"None",Pattren,"0",Pattren,"0",Pattren,"100",Pattren,"farhan.khalid@lums.edu.pk",Pattren,"farhan.khalid@lums.edu.pk");
SendParameters.Body = ParameterValue;
Response = SendParameters.send(PerceptronApiUrl, Options);



