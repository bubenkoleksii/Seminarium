'use client';

import { FC } from 'react';
import { redirect } from 'next/navigation';

type InRegisterSchoolProps = {
  id: string;
}

const getUrl = (code: string) => {
  const api = `https://adm.tools/action/gov/api/`;
  return `${api}?egrpou=${code}`;
};

const InRegisterSchool: FC<InRegisterSchoolProps> = ({ id }) => {
  const url = getUrl(id);

  redirect(url)
};

export { InRegisterSchool };
