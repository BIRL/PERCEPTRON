function Message = SearchQuery( BaseApiUrl, FtpServerName, FtpUserName, UserName, EmailAddress, Password, FullFileName )

PerceptronApiRegisterUserUrl =  BaseApiUrl + 'api/user/CallingPerceptronApi_LoginUserWithSearchQuery' %   CallingPerceptronApiRegisterUser'

try
    ftpobj = ftp(FtpServerName, FtpUserName, Password);
    cd(ftpobj);  sf=struct(ftpobj);  sf.jobject.enterLocalPassiveMode();   %https://undocumentedmatlab.com/blog_old/solving-an-mput-ftp-hang-problem
    %%Password Added here....
    mput(ftpobj,FullFileName);
    
catch
    Message = "File is unable to upload on server."
    msgbox('Input file couldn''t uploaded at Server please check your internet connection and data file. If problem still persists report bug on GitHub','CallingPerceptronApi','Modal');
    return;
end


f = ':'

%%% ATTENTION!!! ALL PARAMETER VALUES ARE CASE SENSITIVE
%%% User will add values below...!!!
%Title
JobName = "Default Run";    % User can change the job title to one of their own choice

%FDRCutOff
FDRCutOff = "0.0"; % User can add values from 0-100

%ProteinDatabase : There are two Protein Databases in Perceptron Human & Ecoli
ProteinDatabase = "Human"; % User can also, select "Ecoli" as well 

%MassMode
MassMode = "M(Neutral)";  % User can also select MH+

%FilterDb
FilterDatabase = "True";     % User can also select "False"

%MwTolerance
ProteinMassTolerance = "500";   % User can select any desired tolerance value

%PeptideTolerance
PeptideTolerance = "15";    % User can select any desired tolerance value

% PeptideToleranceUnit
PeptideToleranceUnit = "ppm";   % User can also select "Da" or "mmu"

% Autotune
TuneIntactProteinMass = "False";    % User can also select "True" to enable intact protein mass tuning

% InsilicoFragType
InsilicoFragmentationType = "HCD";  % User can also select "CID", "ECD", "ETD", "EDD", "BIRD", "SID", "IMD" or "NETD"

% HandleIons
SpecialIons = "bo,bstar,yo,ystar";   
% User can select any or all of "bo,bstar,yo,ystar" for fragmentation types CID, IMD, BIRD, SID and HCD
% User can select any or all of "zo,zoo" for fragmentation types ECD and ETD
% User can select any or all of "ao,astar" for fragmentation types NETD and EDD
% Note: Order must be maintained while adding special ions

% DenovoAllow
DenovoAllow = "True";    % User can also select "False" to disable denovo sequencing

% MinimumPstLength
MinimumPeptideSequenceTagLength = "3";  % User can select any value ranging from 2 to 6

% MaximumPstLength
MaximumPeptideSequenceTagLength = "6"; % User can select any value ranging from 3 to 8

% HopThreshhold
PeptideSequenceTagHopThreshhold = "0.1";  % User can select any decimal value

% Hop_Tolerance_Unit
PeptideSequenceTag_Hop_Tolerance_Unit = "Da";  % User is only allowed to choose Dalton as units

% PSTTolerance
OverallPeptideSequenceTagTolerance = "0.45";    % User can select any decimal value

% Truncation
Truncation = "False";   % User can also select "True" to enable truncation of proteins

%TerminalModification
TerminalModification = "None,NME,NME_Acetylation,M_Acetylation";    % User can select any or all of these terminal modifications. Here NME is for N-Methionine Excision, NME_Acetylation is for N-Methionine Excision and Acetylation, and M_Acetylation is for N-Methionine Acetylation
% Note: Order must be maintained when adding terminal modifications

% PtmAllow
PostTranslationalModificationsAllow = "False";  % User can also select "True"to enable protein search on basis of protein translational modifications

% PtmTolerance
PostTranslationalModificationsTolerance = "0.5";    % User can select any decimal value

% List_of_Modifications
List_of_Modifications = "-";

% Variable_Modifications
Variable_Modifications = "Acetylation_A,Acetylation_K";    % User can select any or all modifications from "Acetylation_A,Acetylation_K,Acetylation_S,Amidation_F,Hydroxylation_P,Methylation_K,Methylation_R,N_Linked_Glycosylation_N,O_Linked_Glycosylation_T,O_Linked_Glycosylation_S,Phosphorylation_S,Phosphorylation_T,Phosphorylation_Y"
                                 % Variable_Modifications and Fixed_Modification should not be conflict with each other
% MethionineChemicalModification
MethionineChemicalModification = "None";    % User can only select anyone "None","MSO", or "MSONE" where MSO being for Methionine Sulfoxide, and MSONE being for Methionine Sulfone 
% Note: Order must be maintained when adding methionine chemical modifications

% Fixed_Modification
Fixed_Modification = "-";     % User can select any or all modifications from "Acetylation_A,Acetylation_K,Acetylation_S,Amidation_F,Hydroxylation_P,Methylation_K,Methylation_R,N_Linked_Glycosylation_N,O_Linked_Glycosylation_T,O_Linked_Glycosylation_S,Phosphorylation_S,Phosphorylation_T,Phosphorylation_Y"
                              % Fixed_Modification and Variable_Modifications should not be conflict with each other
% CysteineChemicalModification
CysteineChemicalModification = "None";  % User can select any or all of "None,Cys_CAM,Cys_PE,Cys_CM,Cys_PAM" where Cys_CAM being for Carboxyamidomethyl Cysteine, Cys_PE being for Pyridyl-Ethyl Cysteine, Cys_CM for Carboxymethyl Cysteine, and Cys_PAM for Propionamide Cysteine
% Note: Order must be maintained when adding cysteine chemical modifications

% MwSweight
MwScoringWeightage = "0";   % User can select the scoring weightage from 0 to 100

% PstSweight
PeptideSequenceTagScoringWeightage = "0";   % User can select the scoring weightage from 0 to 100

% InsilicoSweight
InsilicoScoringWeightage = "100";   % User can select the scoring weightage from 0 to 100

VariableModifications = "-";

FixedModifications = "-";

%%% User will add values above...!!!

[~,FileName,ext] = fileparts(FullFileName);
FileNameWext = string (FileName) + string(ext);  %%% FileName Should not be greater than 15 Characters

ParameterValue = strcat(JobName,f,FDRCutOff,f,ProteinDatabase,f,MassMode,f,FilterDatabase,f,...
    ProteinMassTolerance,f,PeptideTolerance,f,PeptideToleranceUnit,f,TuneIntactProteinMass,f,...
    InsilicoFragmentationType,f,SpecialIons,f,DenovoAllow,f,MinimumPeptideSequenceTagLength,f,...
    MaximumPeptideSequenceTagLength,f,PeptideSequenceTagHopThreshhold,f,PeptideSequenceTag_Hop_Tolerance_Unit,...
    f,OverallPeptideSequenceTagTolerance,f,Truncation,f,TerminalModification,f,PostTranslationalModificationsAllow,f,...
    PostTranslationalModificationsTolerance,f,List_of_Modifications,f,Variable_Modifications,f,...
    MethionineChemicalModification,f,Fixed_Modification,f,CysteineChemicalModification,f,MwScoringWeightage,...
    f,PeptideSequenceTagScoringWeightage,f,InsilicoScoringWeightage,f,VariableModifications,f,FixedModifications,...
    f,UserName,f,EmailAddress,f,Password, f,FileNameWext);


import matlab.net.http.*
SendParameterAndUserInfo = RequestMessage('POST',[]);

SendParameterAndUserInfo.Body = ParameterValue;
Options = matlab.net.http.HTTPOptions('ConnectTimeout',1000);  %% 1000sec
Response = SendParameterAndUserInfo.send(PerceptronApiRegisterUserUrl, Options);

Message = Response.Body.Data;

% For Detailed Error Message if available
% if (string(Message.ExceptionMessage) ~= "")
%     Message = string (Message.ExceptionMessage);
% end
end

