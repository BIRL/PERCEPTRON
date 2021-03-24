function Message = VerfiyingEmailAddress( BaseApiUrl,  UserName, EmailAddress, Password, UserUniqueId)

% Use this function for Email Verfification (after the successfully execution of RegisterUser.m)

import matlab.net.http.*
SendUserInfoForVerify = RequestMessage('POST',[]);

PerceptronApiRegisterUserUrl =  BaseApiUrl + 'api/user/CallingPerceptronApi_VerfiyingEmailAddress';

f = ':'

UserRegisterationInfo = strcat(UserName, f, EmailAddress, f, Password, f, UserUniqueId);
SendUserInfoForVerify.Body = UserRegisterationInfo;
Options = matlab.net.http.HTTPOptions('ConnectTimeout',1000);  %% 1000sec
Response = SendUserInfoForVerify.send(PerceptronApiRegisterUserUrl, Options);

Message = Response.Body.Data;


end

