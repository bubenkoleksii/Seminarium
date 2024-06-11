'use client';

import { InputTextModal } from '@/components/modal/InputTextModal';
import { type Editor } from '@tiptap/react';
import {
  AlignCenter,
  AlignLeft,
  AlignRight,
  Bold,
  Code,
  Heading2,
  Italic,
  Link,
  List,
  ListOrdered,
  Quote,
  Redo,
  Strikethrough,
  Underline,
  Undo,
  Youtube,
} from 'lucide-react';
import { useTranslations } from 'next-intl';
import { useState } from 'react';
import { toast } from 'react-hot-toast';

type Props = {
  editor: Editor | null;
};

const Toolbar = ({ editor }: Props) => {
  const [youtubeModalOpen, setYoutubeModalOpen] = useState(false);
  const [linkModalOpen, setLinkModalOpen] = useState(false);
  const t = useTranslations('Tiptap');

  if (!editor) {
    return null;
  }

  const handleYoutubeModalOpen = () => {
    setYoutubeModalOpen(true);
  };

  const handleLinkModalOpen = () => {
    setLinkModalOpen(true);
  };

  const handleYoutubeModalClose = (confirmed: boolean, text: string) => {
    setYoutubeModalOpen(false);

    if (!confirmed) {
      return;
    }

    if (!text.includes('youtu')) {
      toast.error(t('invalidYoutubeLink'));
      return;
    } else {
      editor.commands.setYoutubeVideo({
        src: text,
        width: 320,
        height: 240,
      });
    }
  };

  const handleLinkModalClose = (confirmed: boolean, text: string) => {
    setLinkModalOpen(false);

    if (text === null || !confirmed) {
      return;
    }

    if (text === '') {
      editor.chain().focus().extendMarkRange('link').unsetLink().run();

      return;
    }

    editor
      .chain()
      .focus()
      .extendMarkRange('link')
      .setLink({ href: text })
      .run();
  };

  const buttonClass = (isActive: boolean) =>
    isActive ? 'rounded-lg bg-purple-950 p-2 text-white' : 'text-gray-800 p-2';

  const iconColor = (isActive: boolean) => (isActive ? 'white' : '#333');

  return (
    <div className="flex w-full flex-wrap items-start justify-between gap-5 rounded-tl-md rounded-tr-md border border-gray-200 px-4 py-3">
      <div className="flex w-full flex-wrap items-center justify-start gap-5 lg:w-8/12">
        <div>
          <input
            type="color"
            onInput={(event) =>
              editor.chain().focus().setColor(event.target.value).run()
            }
            value={editor.getAttributes('textStyle').color}
            data-testid="setColor"
          />
        </div>

        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleBold().run();
          }}
          className={buttonClass(editor.isActive('bold'))}
        >
          <Bold
            color={iconColor(editor.isActive('bold'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleItalic().run();
          }}
          className={buttonClass(editor.isActive('italic'))}
        >
          <Italic
            color={iconColor(editor.isActive('italic'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleUnderline().run();
          }}
          className={buttonClass(editor.isActive('underline'))}
        >
          <Underline
            color={iconColor(editor.isActive('underline'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleStrike().run();
          }}
          className={buttonClass(editor.isActive('strike'))}
        >
          <Strikethrough
            color={iconColor(editor.isActive('strike'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleHeading({ level: 2 }).run();
          }}
          className={buttonClass(editor.isActive('heading', { level: 2 }))}
        >
          <Heading2
            color={iconColor(editor.isActive('heading', { level: 2 }))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleBulletList().run();
          }}
          className={buttonClass(editor.isActive('bulletList'))}
        >
          <List
            color={iconColor(editor.isActive('bulletList'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleOrderedList().run();
          }}
          className={buttonClass(editor.isActive('orderedList'))}
        >
          <ListOrdered
            color={iconColor(editor.isActive('orderedList'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleBlockquote().run();
          }}
          className={buttonClass(editor.isActive('blockquote'))}
        >
          <Quote
            color={iconColor(editor.isActive('blockquote'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().setCode().run();
          }}
          className={buttonClass(editor.isActive('code'))}
        >
          <Code
            color={iconColor(editor.isActive('code'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().setTextAlign('left').run();
          }}
          className={buttonClass(editor.isActive({ textAlign: 'left' }))}
        >
          <AlignLeft
            color={iconColor(editor.isActive({ textAlign: 'left' }))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().setTextAlign('center').run();
          }}
          className={buttonClass(editor.isActive({ textAlign: 'center' }))}
        >
          <AlignCenter
            color={iconColor(editor.isActive({ textAlign: 'center' }))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().setTextAlign('right').run();
          }}
          className={buttonClass(editor.isActive({ textAlign: 'right' }))}
        >
          <AlignRight
            color={iconColor(editor.isActive({ textAlign: 'right' }))}
            className="h-5 w-5"
          />
        </button>
        <InputTextModal
          open={linkModalOpen}
          text={t('linkLabelModal')}
          required={false}
          value={editor.getAttributes('link').href}
          isTextarea={true}
          onClose={handleLinkModalClose}
        />
        <button
          onClick={(e) => {
            e.preventDefault();
            handleLinkModalOpen();
          }}
          className={buttonClass(editor.isActive('link'))}
        >
          <Link
            color={iconColor(editor.isActive('link'))}
            className="h-5 w-5"
          />
        </button>
        <InputTextModal
          open={youtubeModalOpen}
          text={t('youtubeLabelModal')}
          required={true}
          isTextarea={true}
          onClose={handleYoutubeModalClose}
        />
        <button
          onClick={(e) => {
            e.preventDefault();
            handleYoutubeModalOpen();
          }}
          className={buttonClass(editor.isActive('youtube'))}
        >
          <Youtube
            color={iconColor(editor.isActive('youtube'))}
            className="h-5 w-5"
          />
        </button>
      </div>

      <div className="flex w-full items-center justify-start gap-5 lg:w-3/12 lg:justify-end">
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().undo().run();
          }}
          className={buttonClass(editor.isActive('undo'))}
        >
          <Undo
            color={iconColor(editor.isActive('undo'))}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().redo().run();
          }}
          className={buttonClass(editor.isActive('redo'))}
        >
          <Redo
            color={iconColor(editor.isActive('redo'))}
            className="h-5 w-5"
          />
        </button>
      </div>
    </div>
  );
};

export { Toolbar };

