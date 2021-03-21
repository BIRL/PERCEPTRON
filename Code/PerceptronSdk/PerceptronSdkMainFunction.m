function [] = MainCallingPerceptronApi( )

BaseApiUrl = "https://perceptron.lums.edu.pk/PerceptronAPI/"; %%  "http://localhost:52340/";%%

UserName = "Farhan";
EmailAddress = "farhan.biomedical.2022@gmail.com";
Password = "1";

%%% For Guest User Credential  
% UserName = "Guest";
% EmailAddress = "Guest";    % User can add here email address, if user wants to receive an email once results are available
% DummyPassword = "WelcomePerceptron";

%% Use RegisterUser for signup, only needed to execute for one time
%Message = RegisterUser( BaseApiUrl, UserName,  EmailAddress, DummyPassword);

%% Use VerfiyingEmailAddress for email verification, only needed to execute for one time
%Message = VerfiyingEmailAddress( BaseApiUrl,  UserName, EmailAddress, DummyPassword, UserUniqueId);


FtpServerName = 'perceptron.lums.edu.pk';
FtpUserName =  char("perceptron.lums.edu.pk" + "|" + UserName) ;
FullFileName = 'HELA_pk13_sw1_66sc_mono.txt'; % Please add here the path alongwith filename

%% For Submitting query and uploading file on FTP
%Message = SearchQuery( BaseApiUrl, FtpServerName, FtpUserName, UserName, EmailAddress, Password, FullFileName )

%% For downloading results
Message = ResultsDownload( BaseApiUrl, FtpServerName, FtpUserName, UserName, EmailAddress, Password )

%% Messages will be displayed here
disp (Message)
end