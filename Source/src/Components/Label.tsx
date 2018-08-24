import * as React from 'react';

export interface Props {
    name: string,
    color: string,
    description: string
}


function Label({ name, color, description }: Props) {
    return (
        <section>
            <h1>{name}</h1>
            <p>{color}</p>
            <p>{description}</p>
        </section>
    );
}

export default Label;