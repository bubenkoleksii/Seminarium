import { FC } from 'react';
import { Forbidden, Unauthorized } from '@/components/error';

type Props = {
  params: {
    reason: string;
  };
};

const AccessDeniedPage: FC = ({ params }: Props) => {
  return (
    <div className="p-3">
      {params.reason === '401' ? <Unauthorized /> : <Forbidden />}
    </div>
  );
};

export default AccessDeniedPage;
