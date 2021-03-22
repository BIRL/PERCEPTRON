function [] = MainCallingPerceptronApi( )

BaseApiUrl = "https://perceptron.lums.edu.pk/PerceptronAPI/"; %%  

%%% For Guest User Credential  
% UserName = "Guest";
% EmailAddress = "Guest";    % User can add here email address, if user wants to receive an email once results are available
% Password = "WelcomePerceptron";

try
%% Use RegisterUser for signup, only needed to execute for one time
%Message = RegisterUser( BaseApiUrl, UserName,  EmailAddress, Password);

%% Use VerfiyingEmailAddress for email verification, only needed to execute for one time
%Message = VerfiyingEmailAddress( BaseApiUrl,  UserName, EmailAddress, Password, UserUniqueId);

FtpServerName = 'perceptron.lums.edu.pk';
FtpUserName =  char("perceptron.lums.edu.pk" + "|" + UserName) ;
FullFileName = 'HELA_pk13_sw1_66sc_mono.txt'; % Please add here the complete path of your input file


%% For Submitting query and uploading file on FTP
%Message = SearchQuery( BaseApiUrl, FtpServerName, FtpUserName, UserName, EmailAddress, Password, FullFileName )

%% For downloading results
%Message = ResultsDownload( BaseApiUrl, FtpServerName, FtpUserName, UserName, EmailAddress, Password )

%% Messages will be displayed here
disp (Message)
catch exception
    msgbox ('Warning: Something went wrong. If problem still persists please report an issue on GitHub.','PerceptronSdk','Modal');
end
end