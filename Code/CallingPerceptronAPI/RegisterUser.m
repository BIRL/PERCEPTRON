function Message = RegisterUser( BaseApiUrl, UserName,  EmailAddress, DummyPassword)

% Please use this function for Signing Up at Calling PERCEPTRON API
% After successfully execution of this function you would recieve an email for verifying your email address.
% In Email you will recieve a UserUniqueId value 

import matlab.net.http.*
SendUserInfo = RequestMessage('POST',[]);

PerceptronApiRegisterUserUrl =  BaseApiUrl + 'api/user/CallingPerceptronApi_RegisterUser' %   CallingPerceptronApiRegisterUser'

f = ':'

UserRegisterationInfo = strcat(UserName, f, EmailAddress, f, DummyPassword);
SendUserInfo.Body = UserRegisterationInfo;
Options = matlab.net.http.HTTPOptions('ConnectTimeout',1000);  %% 1000sec
Response = SendUserInfo.send(PerceptronApiRegisterUserUrl, Options);

Message = Response.Body.Data;

end

