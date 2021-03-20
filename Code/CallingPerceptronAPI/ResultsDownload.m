function Message = ResultsDownload( BaseApiUrl, UserName, EmailAddress, Password )

%Dear User, Result file would be zip file with the name of User's job title
% Also, if two queries 

CredentialInfo = importdata("C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronFtpInfo.txt");
FtpServerName =  char(CredentialInfo(1,1)); %%string (CredentialInfo.textdata{1, 1});% + string(CredentialInfo.data);
ResultFileName = char("Default Run" + ".zip");

ResultFilesFolder = char(pwd + "\ResultFiles\");

try
    ftpobj = ftp(FtpServerName, UserName, Password);
    mget(ftpobj,ResultFileName, ResultFilesFolder);
catch
    Message = "File is unable to download on server."
    msgbox('Input file couldn''t downloaded at Server please check your internet connection and data file. If problem still persists report bug on GitHub','CallingPerceptronApi','Modal');
    return;
end
end

