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

  return (
    <div
      className="flex w-full flex-wrap items-start justify-between gap-5 rounded-tl-md
    rounded-tr-md border border-gray-700 px-4 py-3"
    >
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
          className={
            editor.isActive('bold')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Bold
            color={editor.isActive('bold') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleItalic().run();
          }}
          className={
            editor.isActive('italic')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Italic
            color={editor.isActive('italic') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleUnderline().run();
          }}
          className={
            editor.isActive('underline')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Underline
            color={editor.isActive('underline') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleStrike().run();
          }}
          className={
            editor.isActive('strike')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Strikethrough
            color={editor.isActive('strike') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleHeading({ level: 2 }).run();
          }}
          className={
            editor.isActive('heading', { level: 2 })
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Heading2
            color={editor.isActive('heading', { level: 2 }) ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleBulletList().run();
          }}
          className={
            editor.isActive('bulletList')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <List
            color={editor.isActive('bulletList') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleOrderedList().run();
          }}
          className={
            editor.isActive('orderedList')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <ListOrdered
            color={editor.isActive('orderedList') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().toggleBlockquote().run();
          }}
          className={
            editor.isActive('blockquote')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Quote
            color={editor.isActive('blockquote') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().setCode().run();
          }}
          className={
            editor.isActive('code')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Code
            color={editor.isActive('code') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().setTextAlign('left').run();
          }}
          className={
            editor.isActive({ textAlign: 'left' })
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <AlignLeft
            color={editor.isActive({ textAlign: 'left' }) ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().setTextAlign('center').run();
          }}
          className={
            editor.isActive({ textAlign: 'center' })
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <AlignCenter
            color={editor.isActive({ textAlign: 'center' }) ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().setTextAlign('right').run();
          }}
          className={
            editor.isActive({ textAlign: 'right' })
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <AlignRight
            color={editor.isActive({ textAlign: 'right' }) ? 'white' : '#333'}
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
          className={
            editor.isActive('link')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Link
            color={editor.isActive('link') ? 'white' : '#333'}
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
          className={
            editor.isActive('youtube')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Youtube
            color={editor.isActive('youtube') ? 'white' : '#333'}
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
          className={
            editor.isActive('undo')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Undo
            color={editor.isActive('undo') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
        <button
          onClick={(e) => {
            e.preventDefault();
            editor.chain().focus().redo().run();
          }}
          className={
            editor.isActive('redo')
              ? 'rounded-lg bg-purple-950 p-2'
              : 'text-sky-400'
          }
        >
          <Redo
            color={editor.isActive('redo') ? 'white' : '#333'}
            className="h-5 w-5"
          />
        </button>
      </div>
    </div>
  );
};

export { Toolbar };

