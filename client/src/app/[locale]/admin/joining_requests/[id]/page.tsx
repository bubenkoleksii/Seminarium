type Props = {
  params: {
    id: string;
  };
};

export default function MyComponent({ params }: Props) {
  return <div>Joining request one {params.id}</div>;
}
