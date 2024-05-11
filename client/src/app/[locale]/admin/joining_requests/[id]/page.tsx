import { JoiningRequest } from '@/features/admin';

type Props = {
  params: {
    id: string;
  };
};

export default function MyComponent({ params }: Props) {
  return (
    <div className="p-3">
      <JoiningRequest id={params.id} />
    </div>
  )
}
