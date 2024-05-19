import { FC } from 'react';
import { HiOutlineExclamationCircle } from 'react-icons/hi';
import { Modal, Button } from 'flowbite-react';
import { useTranslations } from 'next-intl';

interface ProveModalProps {
  open: boolean;
  text: string;
  onClose: (confirmed: boolean) => void;
}

const ProveModal: FC<ProveModalProps> = ({ open, text, onClose }) => {
  const t = useTranslations('Modal');

  return (
    <Modal show={open} size="md" onClose={() => onClose(false)} popup>
      <div className="fixed left-0 top-0 flex h-full w-full items-center justify-center bg-gray-500 bg-opacity-50">
        <div className="max-w-md rounded-lg bg-white p-8 text-center shadow-lg">
          <div className="text-center">
            <HiOutlineExclamationCircle className="mx-auto mb-4 h-14 w-14 text-gray-400 dark:text-gray-200" />
            <h3 className="mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">
              {text}
            </h3>

            <div className="flex justify-center gap-4">
              <Button
                gradientMonochrome="failure"
                onClick={() => onClose(false)}
              >
                <span className="text-white">{t('disprove')}</span>
              </Button>
              <Button
                gradientMonochrome="success"
                onClick={() => onClose(true)}
              >
                <span className="text-white">{t('prove')}</span>
              </Button>
            </div>
          </div>
        </div>
      </div>
    </Modal>
  );
};

export { ProveModal };
