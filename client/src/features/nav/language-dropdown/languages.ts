export const languages: Language[] = [
  {
    name: 'Українська',
    code: 'uk',
    flag: '/languages/uk.png',
  },
  {
    name: 'Qırımtatar',
    code: 'crh',
    flag: '/languages/crh.png',
  },
  {
    name: 'Română',
    code: 'ro',
    flag: '/languages/ro.png',
  },
  {
    name: 'Magyar',
    code: 'hu',
    flag: '/languages/hu.png',
  },
];

interface Language {
  name: string;
  code: string;
  flag: string;
}
