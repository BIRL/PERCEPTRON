function Message = ResultsDownload( BaseApiUrl, FtpServerName, FtpUserName, UserName, EmailAddress, Password )

%Dear User, Result file would be zip file with the name of User's job title

ResultFileName = char("Default Run" + ".zip");

ResultFilesFolder = char(pwd + "\ResultFiles\");

try
    ftpobj = ftp(FtpServerName, FtpUserName, Password);
    cd(ftpobj);  sf=struct(ftpobj);  sf.jobject.enterLocalPassiveMode();
    mget(ftpobj,ResultFileName, ResultFilesFolder);
catch
    Message = "File is unable to download on server."
    msgbox('Input file couldn''t downloaded at Server please check your internet connection and data file. If problem still persists report bug on GitHub','CallingPerceptronApi','Modal');
    return;
end
Message = "File Successfully downloaded and available at provided path."
end

