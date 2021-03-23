function Message = VerfiyingEmailAddress( BaseApiUrl,  UserName, EmailAddress, Password)

% Use this function for Email Verfification (after the successfully execution of RegisterUser.m)

% Dear User please paste below the line of UserUniqueId =
% 'XXX-YYY21-ZZZZ-YZXZ-YZYZYZYZY' as you acquired from the email having subject title is 
% Calling PERCEPTRON API: Email Verification

% UserUniqueId = 'XXX-YYY21-ZZZZ-YZXZ-YZYZYZYZY'; Please insert here your
% User's Unique id for email verification.

import matlab.net.http.*
SendUserInfoForVerify = RequestMessage('POST',[]);

PerceptronApiRegisterUserUrl =  BaseApiUrl + 'api/user/CallingPerceptronApi_VerfiyingEmailAddress' %   CallingPerceptronApiRegisterUser'


f = ':'

UserRegisterationInfo = strcat(UserName, f, EmailAddress, f, Password, f, UserUniqueId);
SendUserInfoForVerify.Body = UserRegisterationInfo;
Options = matlab.net.http.HTTPOptions('ConnectTimeout',1000);  %% 1000sec
Response = SendUserInfoForVerify.send(PerceptronApiRegisterUserUrl, Options);

Message = Response.Body.Data;


end

