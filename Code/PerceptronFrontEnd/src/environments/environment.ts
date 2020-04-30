// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  firebase: {
    apiKey: "AIzaSyCrdVbt7BrEne5lok4F78wjWxDtLNDcQs4",
    authDomain: "proteomics-tdp.firebaseapp.com",
    databaseURL: "https://proteomics-tdp.firebaseio.com",
    projectId: "proteomics-tdp",
    storageBucket: "proteomics-tdp.appspot.com",
    messagingSenderId: "589100138546"
  }
};
