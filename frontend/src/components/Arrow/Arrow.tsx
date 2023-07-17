type TArrowProps = {
    down?: boolean;
    disabled?: boolean;
    onClick?: () => void;
};

export const Arrow = ({ down, disabled, onClick }: TArrowProps) => {
    return (<div
        style={{
            height: '24px',
            transform: `rotate(${down ? '0' : '180'}deg)`,
            margin: '8px',
            cursor: disabled ? 'not-allowed' : 'pointer',
        }}
        onClick={() => !disabled && onClick?.()}>
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M11.725 15.53C11.525 15.53 11.325 15.4299 11.225 15.3298L6.225 10.326C5.925 10.0258 5.925 9.5254 6.225 9.22517C6.525 8.92494 7.025 8.92494 7.325 9.22517L11.825 13.7286L16.325 9.22517C16.625 8.92494 17.125 8.92494 17.425 9.22517C17.725 9.5254 17.725 10.0258 17.425 10.326L12.425 15.3298C12.025 15.4299 11.825 15.53 11.725 15.53Z" fill="#C4C4C4">
            </path>
        </svg>
    </div>)
};
