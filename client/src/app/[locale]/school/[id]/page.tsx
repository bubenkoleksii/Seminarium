import { FC } from 'react';

type Props = {
  params: {
    id: string
  }
}

const SchoolPage: FC<Props> = ({ params }) => {
  return (
    <div>
      {params.id}
    </div>
  );
};

export default SchoolPage;
