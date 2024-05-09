import { FC } from 'react';
import axios from 'axios';

const getData = async () => {
  const res = await axios.get(
    `${process.env.API_GATEWAY_URL}/joiningRequest/getAll/`,
  );

  return res;
};

const JoiningRequestsList: FC = async () => {
  const res = await getData();

  return <div>{JSON.stringify(res)}</div>;
};

export { JoiningRequestsList };
