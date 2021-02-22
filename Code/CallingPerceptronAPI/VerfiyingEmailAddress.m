function Message = VerfiyingEmailAddress( BaseApiUrl )

% Use this function for Email Verfification (after the successfully execution of RegisterUser.m)

% Dear User please paste below the line of UserUniqueId =
% 'XXX-YYY21-ZZZZ-YZXZ-YZYZYZYZY' as you acquired from the email having subject title is 
% Calling PERCEPTRON API: Email Verification
UserUniqueId = '236c4681-a92c-499f-8af8-ea66c283303f'



import matlab.net.http.*
SendUserInfoForVerify = RequestMessage('POST',[]);

PerceptronApiRegisterUserUrl =  BaseApiUrl + 'api/user/CallingPerceptronApi_VerfiyingEmailAddress' %   CallingPerceptronApiRegisterUser'

UserName = "Farhan";
EmailAddress = "farhan.biomedical.2022@gmail.com";
DummyPassword = "12345";
f = ':'

UserRegisterationInfo = strcat(UserName, f, EmailAddress, f, DummyPassword, f, UserUniqueId);
SendUserInfoForVerify.Body = UserRegisterationInfo;
Options = matlab.net.http.HTTPOptions('ConnectTimeout',1000);  %% 1000sec
Response = SendUserInfoForVerify.send(PerceptronApiRegisterUserUrl, Options);

Message = Response.Body.Data;


end

