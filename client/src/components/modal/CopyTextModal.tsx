import { Button, Modal } from 'flowbite-react';
import { useTranslations } from 'next-intl';
import { useQRCode } from 'next-qrcode';
import { FC } from 'react';
import toast from 'react-hot-toast';

interface CopyTextModalProps {
  open: boolean;
  text: string;
  label?: string;
  isNeedQrCode?: boolean;
  onClose: () => void;
}

const CopyTextModal: FC<CopyTextModalProps> = ({
  open,
  text,
  label,
  onClose,
  isNeedQrCode = false,
}) => {
  const t = useTranslations('Modal');
  const { Canvas } = useQRCode();

  const handleCopy = () => {
    navigator.clipboard.writeText(text);

    toast.success(t('copiedSuccess'), { duration: 1500 });
    onClose();
  };

  const handleCancel = () => {
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
            <div className="mb-4 pl-3 pr-3">
              <p className="font-semibold text-blue-500">{text}</p>
            </div>
            {isNeedQrCode && (
              <div className="mb-2 flex justify-center">
                <Canvas
                  text={text}
                  options={{
                    errorCorrectionLevel: 'M',
                    margin: 3,
                    scale: 4,
                    width: 200,
                    color: {
                      dark: '#800080',
                      light: '#F0F0F0',
                    },
                  }}
                />
              </div>
            )}
            <div className="mt-4 flex justify-center gap-4">
              <Button gradientMonochrome="failure" onClick={handleCancel}>
                <span className="text-white">{t('close')}</span>
              </Button>
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

