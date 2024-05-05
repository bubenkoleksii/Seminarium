import { FC } from 'react';
import { Button } from 'flowbite-react';
import { signIn } from 'next-auth/react';

const LoginButton: FC = () => {
  const handleClick = () => {
    signIn('id-server', {
      callbackUrl: '/',
    });
  };

  return (
    <Button onClick={handleClick} outline gradientDuoTone="purpleToPink">
      Login
    </Button>
  );
};

export { LoginButton };
