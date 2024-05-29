import { FC } from 'react';
import { useTranslations } from 'next-intl';
import { Modal, Button } from 'flowbite-react';

interface CopyTextModalProps {
  open: boolean;
  text: string;
  label?: string;
  onClose: () => void;
}

const CopyTextModal: FC<CopyTextModalProps> = ({ open, text, label, onClose }) => {
  const t = useTranslations('Modal');

  const handleCopy = () => {
    navigator.clipboard.writeText(text);
    onClose();
  };

  return (
    <Modal show={open} size="md" onClose={onClose} popup>
      <div className="fixed left-0 top-0 flex h-full w-full items-center justify-center bg-gray-500 bg-opacity-50">
        <div className="max-w-md rounded-lg bg-white p-8 text-center shadow-lg">
          <div className="text-center">
            <h3 className="mb-5 text-lg font-semibold text-gray-800 dark:text-gray-400">
              {label || t('copiedText')}
            </h3>
            <div className="mb-4">
              <p className="text-gray-800">{text}</p>
            </div>
            <div className="flex justify-center gap-4">
              <Button gradientMonochrome="success" onClick={handleCopy}>
                <span className="text-white">{t('copy')}</span>
              </Button>
            </div>
          </div>
        </div>
      </div>
    </Modal>
  );
};

export { CopyTextModal };
