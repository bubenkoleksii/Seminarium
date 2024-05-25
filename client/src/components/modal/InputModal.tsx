import { FC, useState } from 'react';
import { useTranslations } from 'next-intl';
import { Label, Modal, Textarea, Button } from 'flowbite-react';

type Result = { proved: boolean; text: string | null };

interface InputModalProps {
  open: boolean;
  label: string;
  onClose: (result: Result) => void;
  required?: boolean;
}

const InputModal: FC<InputModalProps> = ({
  open,
  label,
  onClose,
  required = false,
}) => {
  const t = useTranslations('Modal');

  const [inputValue, setInputValue] = useState('');
  const [hasError, setHasError] = useState(false);

  const handleSubmit = () => {
    if (required && inputValue.trim() === '') {
      setHasError(true);
    } else {
      setInputValue('');
      setHasError(false);

      onClose({ proved: true, text: inputValue });
    }
  };

  const handleCancel = () => {
    onClose({ proved: false, text: null });
    setHasError(false);
  };

  return (
    <Modal show={open} size="md" onClose={handleCancel} popup>
      <div className="fixed left-0 top-0 flex h-full w-full items-center justify-center bg-gray-500 bg-opacity-50">
        <div className="max-w-md rounded-lg bg-white p-8 text-center shadow-lg">
          <div className="text-center">
            <h3 className="mb-5 text-lg font-semibold text-gray-800 dark:text-gray-400">
              {label}
            </h3>
            <div className="mb-4">
              <Textarea
                id="comment"
                placeholder={t('leaveComment')}
                required={required}
                rows={4}
                value={inputValue}
                onChange={(e) => setInputValue(e.target.value)}
                className={hasError ? 'border-red-500' : ''}
              />
              {hasError && (
                <p className="mt-2 text-sm text-gray-800 text-red-600">
                  {t('required')}
                </p>
              )}
              {!required && (
                <div className="mb-2 block">
                  <Label htmlFor="comment" value={t('optional')} />
                </div>
              )}
            </div>
            <div className="flex justify-center gap-4">
              <Button gradientMonochrome="failure" onClick={handleCancel}>
                <span className="text-white">{t('disprove')}</span>
              </Button>
              <Button gradientMonochrome="success" onClick={handleSubmit}>
                <span className="text-white">{t('prove')}</span>
              </Button>
            </div>
          </div>
        </div>
      </div>
    </Modal>
  );
};

export { InputModal };
