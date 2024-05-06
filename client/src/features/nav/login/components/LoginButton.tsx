import { FC } from 'react';
import { Button } from 'flowbite-react';
import { signIn } from 'next-auth/react';
import { useTranslations } from 'next-intl';

const LoginButton: FC = () => {
  const t = useTranslations('Auth');

  const handle = () => {
    signIn('id-server', {
      callbackUrl: '/',
    });
  };

  return (
    <Button gradientDuoTone="purpleToPink" onClick={handle}>
      <span className="text-white">{t('loginBtn')}</span>
    </Button>
  );
};

export { LoginButton };
