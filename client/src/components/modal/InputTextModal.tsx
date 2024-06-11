import { Button, Label, Modal, TextInput, Textarea } from 'flowbite-react';
import { useTranslations } from 'next-intl';
import React, { FC, useState } from 'react';
import { HiOutlineExclamationCircle } from 'react-icons/hi';

interface InputTextModalProps {
  open: boolean;
  text: string;
  required?: boolean;
  value?: string;
  isTextarea?: boolean;
  onClose: (confirmed: boolean, value?: string) => void;
}

const InputTextModal: FC<InputTextModalProps> = ({
  open,
  text,
  value,
  required = false,
  isTextarea = false,
  onClose,
}) => {
  const t = useTranslations('Modal');
  const [inputValue, setInputValue] = useState(value || '');
  const [hasError, setHasError] = useState(false);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    setInputValue(e.target.value);
    if (required && !e.target.value) {
      setHasError(true);
    } else {
      setHasError(false);
    }
  };

  const handleSubmit = () => {
    if (required && !inputValue) {
      setHasError(true);
      return;
    }
    onClose(true, inputValue);
    setInputValue('');
    setHasError(false);
  };

  const handleCancel = () => {
    onClose(false);
    setInputValue('');
    setHasError(false);
  };

  return (
    <Modal show={open} size="md" onClose={handleCancel} popup>
      <div className="fixed left-0 top-0 flex h-full w-full items-center justify-center bg-gray-500 bg-opacity-50">
        <div className="max-w-lg rounded-lg bg-white p-8 text-center shadow-lg">
          <div className="text-center">
            <HiOutlineExclamationCircle className="mx-auto mb-4 h-14 w-14 text-gray-400 dark:text-gray-200" />
            <h3 className="text-md mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">
              {text}
            </h3>
            <Label className="mb-2 block">
              {isTextarea ? (
                <Textarea
                  value={inputValue}
                  onChange={handleInputChange}
                  rows={6}
                  className="w-full"
                />
              ) : (
                <TextInput
                  value={inputValue}
                  onChange={handleInputChange}
                  className="w-full"
                />
              )}
            </Label>
            {hasError && (
              <p className="text-sm text-red-600">{t('required')}</p>
            )}
            <div className="mt-4 flex justify-center gap-4">
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

export { InputTextModal };
