import { FC } from 'react';
import { Button } from 'flowbite-react';
import { signIn } from 'next-auth/react';
import { useTranslations } from 'next-intl';
import { useMediaQuery } from 'react-responsive';

const LoginButton: FC = () => {
  const t = useTranslations('Auth');
  const isPhone = useMediaQuery({ query: '(max-width: 768px)' });

  const handle = () => {
    signIn('id-server', {
      callbackUrl: '/',
    });
  };

  return (
    <Button gradientDuoTone="purpleToPink"
            onClick={handle}
            size={isPhone ? 'xs' : 'md'}
    >
      <span className="text-white">{t('loginBtn')}</span>
    </Button>
  );
};

export { LoginButton };
