function Message = VerfiyingEmailAddress( BaseApiUrl,  UserName, EmailAddress, DummyPassword, UserUniqueId)

% Use this function for Email Verfification (after the successfully execution of RegisterUser.m)

% Dear User please paste below the line of UserUniqueId =
% 'XXX-YYY21-ZZZZ-YZXZ-YZYZYZYZY' as you acquired from the email having subject title is 
% Calling PERCEPTRON API: Email Verification


import matlab.net.http.*
SendUserInfoForVerify = RequestMessage('POST',[]);

PerceptronApiRegisterUserUrl =  BaseApiUrl + 'api/user/CallingPerceptronApi_VerfiyingEmailAddress' %   CallingPerceptronApiRegisterUser'


f = ':'

UserRegisterationInfo = strcat(UserName, f, EmailAddress, f, DummyPassword, f, UserUniqueId);
SendUserInfoForVerify.Body = UserRegisterationInfo;
Options = matlab.net.http.HTTPOptions('ConnectTimeout',1000);  %% 1000sec
Response = SendUserInfoForVerify.send(PerceptronApiRegisterUserUrl, Options);

Message = Response.Body.Data;


end

