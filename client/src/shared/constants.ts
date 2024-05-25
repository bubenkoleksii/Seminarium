export const routes = {
  getCreateJoiningRequest: (locale) => `${locale}/create-joining-request`,
  registration: 'registration',
};

export const school = {
  type: {
    secondary: 'secondary',
    higher: 'higher',
    vocational: 'vocational',
  },
  ownershipType: {
    state: 'state',
    communal: 'communal',
    private: 'private',
  },
  region: {
    vinnytsia: 'vinnytsia',
    volyn: 'volyn',
    dnipropetrovsk: 'dnipropetrovsk',
    donetsk: 'donetsk',
    zhytomyr: 'zhytomyr',
    zakarpattia: 'zakarpattia',
    zaporizhzhia: 'zaporizhzhia',
    ivanofrankivsk: 'ivanofrankivsk',
    kyiv: 'kyiv',
    kirovohrad: 'kirovohrad',
    luhansk: 'luhansk',
    lviv: 'lviv',
    mykolaiv: 'mykolaiv',
    odesa: 'odesa',
    poltava: 'poltava',
    rivne: 'rivne',
    sumy: 'sumy',
    ternopil: 'ternopil',
    kharkiv: 'kharkiv',
    kherson: 'kherson',
    khmelnytskyi: 'khmelnytskyi',
    cherkasy: 'cherkasy',
    chernivtsi: 'chernivtsi',
    chernihiv: 'chernihiv',
    crimea: 'crimea',
    abroad: 'abroad',
    none: 'none',
  },
};

export const mediaQueries = {
  phone: '(max-width: 768px)',
  desktopOrLaptop: '(min-width: 1280px)',
};
