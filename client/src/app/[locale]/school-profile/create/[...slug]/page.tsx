import { FC } from 'react';

type Props = {
  params: {
    slug: string[];
  };
};

const CreateProfilePage: FC<Props> = ({ params }) => {
  const [type, invitationCode] = params.slug.map((param) => decodeURIComponent(param));

  return (
    <div>
      {type} {invitationCode}
    </div>
  );
};

export default CreateProfilePage;
